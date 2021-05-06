#! /usr/bin/pwsh
function Get-Version {
  return GitVersion.exe | ConvertFrom-Json
}

function Get-Solution-File {
  param (
    $Path = "."
  )

  return Get-ChildItem -Path $Path -Filter *.sln
}

function Get-Services {
  param (
    $Version,
    $Path = "."
  )

  return Get-ChildItem -Path $Path -Filter Dockerfile -Recurse -ErrorAction SilentlyContinue -Force
    | ForEach-Object {
      $Directory = $_.Directory
      $ProjectFile = Get-ChildItem -Path $Directory -Filter *.csproj
      $ServiceName = $ProjectFile.BaseName
      $RepositoryName = $ServiceName.Replace(".", "").ToLower().Trim()
      New-Object PSObject -Property @{
        ProjectDirectory = $Directory;
        ProjectFile = $ProjectFile;
        ServiceName = $ServiceName;
        RepositoryName = "culinacloud/$($RepositoryName)";
        Tag = $Version.SemVer;
        ImageName = "$($RepositoryName):$($Version.SemVer)"
      }
    }
}

function Get-AWS-Account-Info {
  $CallerIdentity = aws.exe sts get-caller-identity | ConvertFrom-Json
  $Region = aws.exe configure get region
  return New-Object PSObject -Property @{
    Region = $Region;
    AccountId = $CallerIdentity.Account
  }
}

function Get-ECR-Repo {
  param (
    $RepositoryName
  )

  $RepoInfo = aws ecr describe-repositories --repository-names $RepositoryName 2>$null
  if (!$RepoInfo) {
    $RepoInfo = aws ecr create-repository --repository-name $RepositoryName
    $RepoInfo = $RepoInfo | ConvertFrom-Json
    $RepoInfo = $RepoInfo.repository
  } else {
    $RepoInfo = $RepoInfo | ConvertFrom-Json
    $RepoInfo = $RepoInfo.repositories[0]
  }
  return $RepoInfo
}

# Calculate Version
$Version = Get-Version

# Get Solution File Info
$SolutionFile = Get-Solution-File

# Build Solution
Write-Output "Building Culina Cloud Services Version: $($Version.SemVer)`n"
dotnet.exe build $SolutionFile.Name
Write-Output "`n"

# Run Tests
Write-Output "Testing Culina Cloud Services Version: $($Version.SemVer)`n"
dotnet.exe test $SolutionFile.Name
Write-Output "`n"

# Login to ECR Repository
$AWSAccountInfo = Get-AWS-Account-Info
aws.exe ecr get-login-password --region $AWSAccountInfo.Region | docker.exe login --username AWS --password-stdin "$($AWSAccountInfo.AccountId).dkr.ecr.$($AWSAccountInfo.Region).amazonaws.com"
Write-Output "`n"

# Deploying Services
$Services = Get-Services -Version $Version
foreach ($Service in $Services) {
  Write-Output "Ensuring that the ECR Repository Exists for the $($Service.ServiceName) Service"
  $RepositoryInfo = Get-ECR-Repo -RepositoryName $Service.RepositoryName

  Write-Output "Packaging the $($Service.ServiceName) Service Version: $($Service.Tag)"
  docker.exe build -t $Service.ImageName -f "$($Service.ProjectDirectory)/Dockerfile" .
  docker.exe tag $Service.ImageName "$($RepositoryInfo.repositoryUri):$($Service.Tag)"
  Write-Output "`n"

  Write-Output "Deploying the $($Service.ServiceName) Service Version: $($Service.Tag)"
  docker.exe push "$($RepositoryInfo.repositoryUri):$($Service.Tag)"
  Write-Output "`n"
}
