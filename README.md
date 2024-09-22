## Background
The Frontend was built with Angular. Backend was done with ASP.NET


## Frontend

Deployed and hosted using netlify. The app is accessible via - 

You can run locally with `ng serve` Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Backend
The Web API project was deployed using Azure. MSSQL database was hosted on Azure 

You can run locally 
Execute `dotnet run` for a dev server. Modify the application.json file as shown below and access the web apis by navigating to http://localhost:5093/swagger/index.html 
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5093"
