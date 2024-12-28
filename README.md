# asp.net
Basic asp.net REST API project with security

# How to run this project?
1. Clone the project in your local machine.
2. Create a database in SQLServer named `finshark` 
3. Update `appsettings.json`
    <ul> 
        <li>Data Source= SET_YOUR_DESKTOP_NAME </li>
        <li>Catalog= SET_DATABASE_NAME </li>
    </ul>
3. Build the project and make sure there are no errors: <br/>
    `dotnet build`
4. Run the migration command: <br/>
   `dotnet ef migrations add init`
5. Run the project and make sure Swagger page is displayed:<br/>
    `dotnet watch run`