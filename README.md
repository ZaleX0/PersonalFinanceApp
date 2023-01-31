# PersonalFinanceApp

// TODO

### Features
- JWT Bearer Authorization

# Configuration
## 1. Startup project
Right click on the `PersonalFinanceApp.Api` project in Solution Explorer and select `Set as Startup Project`

## 2. Database
1. Connection String
    - Change your connection string in [appsettings.json](PersonalFinanceApp.Api/appsettings.json)
```json
"ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=PersonalFinanceApp;Trusted_Connection=True;"
}
```

2. Initiate database
    - Open `Package Manager Console`.
    - Run `update-database` command.
> Sample data would be inserted into database automatically by using the [FinanceSeeder](PersonalFinanceApp.Services/Seeders/FinanceSeeder.cs)

### You can start the API now

## 3. React Client
The way I open client project is to open `personalfinanceapp.client` in Visual Studio Code.
- From there run those two commands in terminal.
    - `npm install`
    - `npm start`
>**Note**
> There might be an windows specific error saying [Plugin "react" was conflicted between "package.json » eslint-config-react-app »](https://stackoverflow.com/questions/70377211/error-when-deploying-react-app-and-it-keeps-sayings-plugin-react-was-confli)

# Architecture
# Database
