FNB Ops is an enterprise-level web application designed to improve restaurant operations through structured data management and intelligent workflows.
The system provides tools for managing ingredients, tracking inventory, and supporting operational decision-making.
This project demonstrates a clean N-Layer architecture using modern web development practices.

1)	To run this application locally, install:
•	.NET 8.0 SDK
•	Git
•	MySQL 8.0 
•	VS Code
2)	Clone Application Source Code from Github
•	.To clone the repo locally: git clone  https://github.com/jinnyanguyen/fnb_ops.git
•	Then cd fnb_ops
•	Confirm the solution structure exist includes: RestaurantOps.sln, RestaurantOps.Web/, RestaurantOps.Business/,RestaurantOps.Data/
,RestaurantOps.Models/, RestaurantOps.Infrastructure/
3)	Database Setup (MySQL) - RestaurantOps requires a relational MySQL database. The database can be created locally for development and hosted on AWS RDS for production deployment.
•	Start MySQL locally then create the database and user in MySQL Workbench
•	Create schema using DDL Script (provided in repo)
•	Validate database, confirm tables exist (Users, Ingredients, Recipes, RecipeIngredients, Sales, Forecasts, TrainingModules, UserTraining, TaskAssignments).
4) Configure connection string in RestaurantOps.Web/appsettings.json
5) Apply Database Migration: dotnet ef database update --project RestaurantOps.Data --startup-project RestaurantOps.Web
6) Run the application: dotnet run --project RestaurantOps.Web
7) Open browser: https://localhost:5001/Ingredient
