# HRIS Integration API on .NET 10
## Structure
- Domain: core entity
- Application: contracts, validators, use cases
- Infrastructure: EF Core 10 and SQL Server
- API: controllers, JWT, Swagger, health check
- Tests: xUnit
## Run
```powershell
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/HRIS.Infrastructure --startup-project src/HRIS.Api --output-dir Migrations
dotnet ef database update --project src/HRIS.Infrastructure --startup-project src/HRIS.Api
dotnet test
dotnet run --project src/HRIS.Api
```
Open http://localhost:5080/swagger. Call POST /api/auth/token with `admin` / `ChangeMe123!`, copy accessToken, click Authorize, and paste the token. Change demo credentials, JWT key, and connection string before deployment. The demo token endpoint is for local development only; production should use the organization identity provider.
