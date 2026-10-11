# Mini Bank — ASP.NET Core MVC + EF Core

Educational mini-project featuring accounts, deposits, withdrawals, transaction history, validation, service layer, and EF Core persistence in SQL Server LocalDB.

## Requirements
- .NET 8 SDK
- SQL Server Express LocalDB (installed with Visual Studio workload or SQL Server Express)

## Run
```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.22
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```
Open the localhost URL shown in the terminal. If `dotnet-ef` is already installed, use `dotnet tool update --global dotnet-ef --version 8.0.22` if needed.

Alternatively, in Visual Studio Package Manager Console, after installing EF Core Tools 8.x, run `Add-Migration InitialCreate` then `Update-Database`.

The DB connection string is in appsettings.json. LocalDB is Windows-only; use an actual SQL Server connection string on other operating systems. Migrations folder is generated with first command.

## Notes
- Not a production banking system: no sign-in, permissions, audit-grade ledger, transfers, compliance, or production-ready concurrency/retry strategy.
- Concurrent edits return an error rather than silently overwriting the account, and balance and transaction are saved together.
- For a public deployment, implement authentication, authorization, stronger database constraints, structured logging, and a production transaction design.
