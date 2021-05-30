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