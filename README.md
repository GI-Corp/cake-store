# CakeStore Docker Setup Guide

Follow these steps to set up and run the CakeStore project using Docker Compose.

## Prerequisites

- Docker Engine installed on your machine.

## Steps

### 1. Clone the Repository

Clone the CakeStore project repository to your local machine:
```bash
git clone <repository_url>
```

### 2. Navigate to the Project Directory

Change your current directory to the root directory of the cloned CakeStore project:
```bash
cd <project_directory>
```

### 3. Set Environment Variables

Create a `.env` file in the project directory and configure the following environment variables:

```plaintext
ENVIRONMENT=Development
TIMEZONE=UTC
POSTGRES_USER=<postgres_user>
POSTGRES_PASSWORD=<postgres_password>
POSTGRES_DB=<postgres_database>
POSTGRES_HOST=database
POSTGRES_PORT=5432
PG_BOUNCER_MAX_CLIENT_CONN=100
PG_BOUNCER_MAX_DB_CONN=90
PG_BOUNCER_AUTH_TYPE=scram-sha-256
COLLECTOR_ENABLED=true
JAEGER_LOG_LEVEL=debug
```

Replace `<postgres_user>`, `<postgres_password>`, and `<postgres_database>` with your desired PostgreSQL credentials.

### 4. Build and Run Docker Containers

Execute the following command to build and start the Docker containers defined in the `docker-compose.yml` file:
```bash
docker-compose up --build
```

### 5. Access CakeStore Application

Once the containers are up and running, you can access the CakeStore application at `http://localhost:8010` in your web browser.

### 6. Access RabbitMQ Management Interface

RabbitMQ management interface is available at `http://localhost:15672`. Login with username `guest` and password `guest`.

### 7. Access Jaeger Tracing Dashboard

Jaeger tracing dashboard is available at `http://localhost:16686`.

### 8. Shut Down the Project

To stop and remove the Docker containers, press `Ctrl + C` in the terminal where `docker-compose` is running. Then, execute the following command:
```bash
docker-compose down
```

## Notes

- Ensure Docker Engine is running and accessible.
- Make sure the required ports (8010, 5432, 5672, 6379, 15672, 16686, etc.) are not already in use.
- Adjust the environment variables and configurations as needed for your environment.
- Refer to the project documentation for any additional configuration or setup instructions.
