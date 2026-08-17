# Gusto Ops Platform

Gusto Ops Platform is a cloud-based restaurant operations management system designed to centralize and improve day-to-day restaurant workflows through structured data management, automation, operational tracking, and external system integration.

The platform provides tools for ingredient and inventory management, recipe execution, sales processing, staff task management, SOP execution, training, multi-branch operations, reporting, and POS data integration.

The application follows a clean N-Layer architecture that separates presentation, business logic, data access, domain models, and external integrations. This architecture improves maintainability, testability, scalability, and extensibility while allowing new integrations and operational features to be added without tightly coupling them to the core application.

Gusto Ops Platform is developed using ASP.NET Core MVC and Entity Framework Core, backed by MySQL, deployed to AWS Elastic Beanstalk, and supported by GitHub Actions for automated build, testing, and deployment.

---

## 1. Technology Stack

The project uses the following technologies:

- ASP.NET Core MVC
- C#
- .NET 8
- Entity Framework Core
- MySQL 8
- Pomelo Entity Framework Core MySQL Provider
- HTML, CSS, JavaScript, and Bootstrap
- CsvHelper
- xUnit automated testing
- Git and GitHub
- GitHub Actions
- AWS Elastic Beanstalk
- AWS-hosted MySQL database for cloud deployment

---

## 2. Application Architecture

Gusto Ops Platform follows an N-Layer architecture:

### RestaurantOps.Web
Presentation layer containing ASP.NET Core MVC controllers, views, application configuration, dependency injection, and web endpoints.

### RestaurantOps.Business
Business/service layer containing application workflows, validation rules, inventory processing, sales processing, SOP execution, recipe execution, and external-sale processing.

### RestaurantOps.Data
Data-access layer containing Entity Framework Core database configuration, repositories, transaction management, and persistence logic.

### RestaurantOps.Models
Contains shared domain models, entities, commands, results, and integration models.

### RestaurantOps.Integrations
Integration layer responsible for vendor-neutral external data processing, including CSV sales parsing and future POS adapters.

### RestaurantOps.Tests
Automated test project used to verify business rules, CSV parsing, integrations, and other application functionality.

The general application flow is:

User / External System
        ↓
ASP.NET Core MVC
        ↓
Controller
        ↓
Business Service
        ↓
Repository / Data Access
        ↓
Entity Framework Core
        ↓
MySQL

External sales follow an additional integration path:

CSV / Future POS Provider
        ↓
Integration Adapter / Parser
        ↓
Vendor-Neutral Import Model
        ↓
Imported Sale Service
        ↓
Existing Sales & Inventory Logic
        ↓
MySQL

---

## 3. Core Functionality

Gusto Ops Platform currently supports:

- User authentication
- Role-based access for managers and staff
- Ingredient management
- Inventory tracking
- Inventory transaction history
- Recipe management
- Recipe ingredient relationships
- Recipe execution workflows
- Automatic inventory deduction
- Sales processing
- Staff task assignment and tracking
- SOP management
- SOP execution workflows
- Training functionality
- Multi-branch restaurant operations
- Operational dashboard functionality
- Staff performance tracking
- External branch mapping
- External recipe/item mapping
- Vendor-neutral POS integration architecture
- Imported-sale processing
- Duplicate external-sale prevention
- Transactional external-sale processing
- CSV sales import
- CSV validation and parsing
- Automated testing
- Cloud deployment through AWS Elastic Beanstalk
- CI/CD workflow through GitHub Actions

The POS integration architecture is intentionally vendor-neutral so future providers such as Toast, Square, or other restaurant POS platforms can be integrated without replacing the core sales and inventory business logic.

---

# Local Development Setup

## 4. Prerequisites

Install the following software before running the application locally:

- .NET 8 SDK
- Git
- MySQL 8
- MySQL Workbench
- Visual Studio Code or Visual Studio
- Entity Framework Core CLI tools

Verify .NET:

    dotnet --version

Verify Git:

    git --version

Verify Entity Framework tools:

    dotnet ef --version

If the EF Core CLI is not installed:

    dotnet tool install --global dotnet-ef

---

## 5. Clone the Repository

Clone the application from GitHub:

    git clone https://github.com/jinnyanguyen/fnb_ops.git

Navigate into the repository:

    cd fnb_ops

The solution contains the following primary projects:

    RestaurantOps.slnx
    RestaurantOps.Web/
    RestaurantOps.Business/
    RestaurantOps.Data/
    RestaurantOps.Models/
    RestaurantOps.Integrations/
    RestaurantOps.Tests/

---

## 6. Database Setup

RestaurantOps requires a MySQL relational database.

MySQL can be hosted:

- Locally for development and testing
- Remotely for cloud deployment

Start the local MySQL server and create the required database and database user.

Create the database schema using the project's database scripts and/or Entity Framework Core migrations.

The database contains tables supporting areas such as:

- Users
- Ingredients
- Recipes
- Recipe ingredients
- Sales
- Inventory transactions
- Task assignments
- SOPs and SOP execution
- Training
- Branch operations
- External branch mappings
- External recipe mappings
- Imported sale records

Additional tables may exist depending on the current migration version.

---

## 7. Configure the Database Connection

Configure the local development connection string using the application's configuration/environment settings.

The ASP.NET Core project is located at:

    RestaurantOps.Web/

Production credentials should be supplied through secure environment configuration rather than stored directly in source control.

---

## 8. Apply Entity Framework Core Migrations

From the repository root, run:

    dotnet ef database update --project RestaurantOps.Data --startup-project RestaurantOps.Web

This applies the current Entity Framework Core migrations to the configured MySQL database.

---

## 9. Build the Application

Build the complete solution:

    dotnet build RestaurantOps.slnx

A successful build should end with:

    Build succeeded.

---

## 10. Run Automated Tests

Run the automated test suite:

    dotnet test RestaurantOps.Tests/RestaurantOps.Tests.csproj

All production changes should be validated through the appropriate automated and manual tests before deployment.

---

## 11. Run the Application Locally

From the repository root:

    dotnet run --project .\RestaurantOps.Web\RestaurantOps.Web.csproj

The console will display the local URL assigned by ASP.NET Core.

For the current development configuration, this may be similar to:

    http://localhost:5031

Open the URL displayed in the terminal rather than assuming a fixed port.

---

# CSV / POS Integration

## 12. CSV Sales Import

The CSV integration provides a vendor-neutral method for importing external restaurant sales into Gusto Ops.

The import workflow performs the following operations:

1. Accepts external sales data.
2. Parses and validates the CSV data.
3. Converts external records into vendor-neutral integration models.
4. Resolves the external store to an internal branch.
5. Resolves external menu items to internal recipes.
6. Checks whether the external sale was previously imported.
7. Processes the sale through existing business services.
8. Deducts inventory using existing inventory logic.
9. Records the import result.
10. Prevents successful external sales from being imported twice.

This architecture allows future POS adapters to reuse the same internal workflow.

---

# Cloud Deployment

## 13. AWS Deployment

Gusto Ops Platform is deployed using AWS Elastic Beanstalk.

The production deployment uses a self-contained Linux x64 build of the .NET 8 application.

A self-contained deployment packages the required .NET runtime with the application, reducing dependency on the .NET runtime installed by the Elastic Beanstalk platform.

The production publish command is:

    dotnet publish RestaurantOps.Web/RestaurantOps.Web.csproj \
      --configuration Release \
      --runtime linux-x64 \
      --self-contained true \
      --output publish

The Elastic Beanstalk deployment package includes a Procfile:

    web: ./RestaurantOps.Web

This instructs Elastic Beanstalk to launch the self-contained Linux executable.

---

## 14. Continuous Integration and Deployment

GitHub Actions is used to automate the build, test, publish, and AWS deployment workflow.

The CI/CD pipeline follows this process:

    Push to main
         ↓
    GitHub Actions
         ↓
    Restore Dependencies
         ↓
    Build Solution
         ↓
    Run Automated Tests
         ↓
    Publish Self-Contained Linux Application
         ↓
    Verify Deployment Output
         ↓
    Deploy to AWS Elastic Beanstalk

Automated tests are executed before deployment so a failing test can prevent an invalid application build from progressing to the deployment stage.

AWS credentials used by the workflow must be stored securely using GitHub repository secrets and must never be committed to source control.

---

## 15. Build and Deployment Artifacts

Generated build and deployment files are excluded from Git source control.

Examples include:

    bin/
    obj/
    publish/
    *.zip

These files are generated locally or by the CI/CD pipeline and should not be committed to the repository.

---

## 16. Future Enhancements

The architecture supports future expansion including:

- Direct POS API integrations
- Additional POS provider adapters
- Automated sales synchronization
- Enhanced operational analytics
- Advanced staff performance reporting
- Additional multi-branch analytics
- Inventory forecasting
- Automated purchasing recommendations
- Expanded training and compliance reporting
- Additional cloud monitoring and alerting
