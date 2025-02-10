# Price Negotiator System

The Price Negotiator System is an application designed for price negotiation and management. Below are the steps to set up and run the project using Docker.

---

## 🚀 Setup Instructions

### 1. Clone or Download the Repository

Clone this repository to your local machine or download the project files:

```bash
git clone https://your-repo-url.git
```

### 2. Open the appsettings.json and configure required settings. For Example set up the database connection string as follows:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqldata;Database=PriceNegotiatorDatabase;User Id=SA;Password=Pass@word;Encrypt=false;TrustServerCertificate=true;Integrated Security=false;"
  }
}
```

### 3. Run the project with Docker.

```bash
docker-compose up --d
```
This will be build the application.

### 4. Wait for initialization
The application runs automated database migrations at startup and depends on the database being ready.
Please wait 10 seconds after starting the services for the database to initialize before the application begins running.

### 5. Access the application
Once initialized, the application will be available at:

http://localhost:5268/swagger/index.html

This Swagger interface allows you to test and interact with the available API endpoints.