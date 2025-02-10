# Price Negotiator System

The Price Negotiator System is an application designed for price negotiation and management. Below are the steps to set up and run the project using Docker.

---

## 🚀 Setup Instructions

### 1. Clone or Download the Repository

Clone this repository to your local machine or download the project files:

```bash
git clone https://github.com/Evas122/Product-Price-Negotiator-System
```

### 2. Configure the application

Open the appsettings.json file in the src/PriceNegotiator.Api directory and provide the necessary configuration values, such as connection strings or other application settings.

Example configuration:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqldata;Database=PriceNegotiatorDatabase;User Id=SA;Password=Pass@word;Encrypt=false;TrustServerCertificate=true;Integrated Security=false;"
  }
}
```

### 3. Run the project with Docker

Navigate to the root directory of the project (where the docker-compose.yml file is located) and run the following command:

```bash
docker-compose up --d
```
This command will:

1. Build the application.
2. Start the database service (sqldata).
3. Start the application service (price-negotiator-app).

### 4. Wait for initialization
The application has automated database migrations that run on startup. It will wait for the database to be ready before applying migrations.

Important: After starting the services, please wait 10 seconds for the database to fully initialize before the application starts.

### 5. Access the application
Once everything is ready, the application will be accessible at the following URL:

http://localhost:5268/swagger/index.html

You can use the Swagger interface to explore and test the available endpoints.