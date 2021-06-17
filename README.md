# Culina Cloud Services

A cloud based kitchen companion app for aggerating, sharing, and recommending recipes based on user's preferences

# Development

## Useful Commands

### Create EF Core Migrations
```
dotnet ef migrations add "InitialCreate" --project ./CookBook.Infrastructure --startup-project ./CookBook.API --output-dir Persistence/Migrations
```

### Start Postgresql 10 Docker Container Locally

```
docker run -v postgres-data:/var/lib/postgresql/data -p 5432:5432 --name CulinaCloudDB -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=postgres -d postgres:10
```

### Export Postgresql Data Dumps from Docker Container

```
docker exec -it {containerId} pg_dump -f /{dumpFileName}.dump -Fc --no-acl --no-owner --data-only -U {masterUser} {databaseName} --schema '"{schemaName}"'

Example:

docker exec -it a2f5ad6981a0 pg_dump -f /cookbook_culinacloud_db.dump -Fc --no-acl --no-owner --data-only -U postgres CulinaCloudDB --schema '"CookBook"'
```

### Copy Postgresql Data Dumps from Docker Container

```
docker cp {containerName}:/{dumpFileName} {dumpFileName}

Example:

docker cp culina-cloud_postgres_1:/cookbook_culinacloud_db.dump cookbook_culinacloud_db.dump
```

### Copy Postgresql Data Dumps into Docker Container

```
docker cp {dumpFileName} {containerName}:/{dumpFileName}

Example:

docker cp cookbook_culinacloud_db.dump culina-cloud_postgres_1:/cookbook_culinacloud_db.dump
```

### Restore Postgresql Data from Data Dump

```
docker exec -it {containerId} pg_restore -U {masterUser} -d {databaseName} -1 {dumpFileName}

Example:

docker exec -it a528f9631b34 pg_restore -U postgres -d CulinaCloudDB -1 /cookbook_culinacloud_db.dump
```
