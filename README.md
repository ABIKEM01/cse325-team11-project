# ServeTogether

CSE 325 Team 11 project. A Blazor web app where ward and stake leaders post service projects and members sign up for time slots.

## Team Members
- Abiodun Babayode
- Amando Evangelista Santos Filho
- Deborah Sarah Okon
- Steven Ricardo Savarin Suarez

## Links
- Trello board (view only): https://trello.com/b/1RMCQ9qr/team-11-cse-325-project

## Tech
Blazor Web App on .NET 8, Entity Framework Core with SQLite, ASP.NET Core Identity.

## Getting started
You need the .NET 8 SDK and the EF Core tool (`dotnet tool install --global dotnet-ef --version 8.0.*`).

```
git clone https://github.com/ABIKEM01/cse325-team11-project
cd cse325-team11-project/ServeTogether
dotnet user-secrets set "SeedLeader:Email" "leader@servetogether.test"
dotnet user-secrets set "SeedLeader:Password" "<a password with upper, lower, digit and symbol>"
dotnet run
```

On first run the app creates its SQLite database, the Leader role, your local leader account, and one sample project. Register any other account to act as a Member.

The database file (`app.db`) is not committed. Everyone gets their own local copy.

## Working together
- Pull before you start: `git pull`
- One branch per Trello card: `git checkout -b feature/<card-name>`
- Push your branch and open a pull request; someone else reviews before it merges to `main`
- If you change a model, add a migration in the same pull request: `dotnet ef migrations add <Name>`
