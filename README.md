# ECE Backend

A robust, production-ready .NET 9 Web API boilerplate following Clean Architecture principles. This project is designed to be a solid foundation for building scalable and maintainable backend systems.

## ðŸ—ï¸ Architecture

The project is structured into four main layers:
- **Domain**: Contains enterprise logic, entities, and value objects.
- **Application**: Contains business logic, features (CQRS), and interfaces.
- **Infrastructure**: Implementation of external concerns like Database (EF Core), Identity, and Caching.
- **Api**: The entry point, handling HTTP requests, OpenApi (Swagger) documentation, and Middleware.

## ðŸš€ Getting Started

### 1. Rename the Project
To personalize this boilerplate, use the provided PowerShell script. This will recursively rename all occurrences of "ECE" in file contents, file names, and directories.

1. Open PowerShell in the root directory.
2. Run the script:
   ```powershell
   .\Rename-Project.ps1
   ```
3. Follow the prompt to enter your new project name (e.g., `MySuperApp`).

### 2. Configure Environment
Create a `.env` file in the `Code` directory with your secrets and configuration. (A template is provided in the next section).

### 3. Run with Docker
The project includes a complete observability stack (Seq, Prometheus, Grafana) and SQL Server.

```bash
cd Code
docker-compose up -d
```

## ðŸ› ï¸ Tech Stack
- **Framework**: .NET 9
- **Database**: SQL Server + Entity Framework Core
- **Identity**: ASP.NET Core Identity with JWT
- **Observability**: Seq (Logging), Prometheus & Grafana (Metrics)
- **Mapping**: AutoMapper
- **Validation**: FluentValidation
- **Messaging/CQRS**: MediatR
- **Testing**: xUnit, FluentAssertions, NSubstitute

## ðŸ§ª Testing
The solution includes:
- **Unit Tests**: Testing individual components in isolation.
- **Subcutaneous Tests**: Testing the application layer through the MediatR pipeline, bypassing the API layer but including the database (using an in-memory or real test database).

## ðŸ“„ Environment Variables (`.env`)
Ensure the following variables are set in your `Code/.env` file:

```env
JWT_KEY=your-secure-32-char-key
EMAIL=youremail@gmail.com
EMAIL_PASSWORD=your-email-app-password
SA_PASSWORD=YourStrongPassword123!
SEQ_ADMIN_PASSWORD=YourSeqPassword
GRAFANA_ADMIN_PASSWORD=YourGrafanaPassword
DB_CONNECTION=Server=sqlserver,1433;Database=YourDbName;User=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;MultipleActiveResultSets=True
```

