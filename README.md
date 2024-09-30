## Background
The Action Scheduler is designed for managing scheduled tasks efficiently.
The Frontend was built with Angular. Backend was done with ASP.NET


## Frontend

### Deployment
The frontend is deployed and hosted using Netlify. The application is accessible via the following URL:
- [Action Scheduler - Frontend](https://actionscheduler.vercel.app/)


You can run locally with `ng serve` Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.


## Backend

### Deployment
The Web API project was deployed using Render, utilizing PostgreSQL as the database. The backend was containerized and deployed using Docker.

### Accessing the API
You can access the API documentation via Swagger at the following URL:
- [API Documentation](https://todolist-qlng.onrender.com/swagger/index.html)

### Running Locally
To run the backend locally, 
Execute `dotnet run` for a dev server. Modify the launchSettings.json file as shown below and access the web apis by navigating to http://localhost:5093/swagger/index.html 
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5093"
