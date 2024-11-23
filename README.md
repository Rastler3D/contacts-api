# Contacts Management API

This project is a simple REST API for managing contacts, built with ASP.NET Core and Entity Framework Core.

## Features

- CRUD operations for contacts
- Pagination support
- Database storage with PostgreSQL
- Dockerized application and database
- Unit tests
- Postman collection for API testing

## Prerequisites

- Docker
- Docker Compose
- .NET 8.0 SDK

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/Rastler3D/contacts-api.git
   cd contacts-api
   ```

2. Build and run the Docker containers:
   ```
   docker-compose up --build
   ```

3. The API will be available at `http://localhost:8080`

## API Endpoints

- GET /api/contacts - Get all contacts (with pagination)
- GET /api/contacts/{id} - Get a specific contact
- POST /api/contacts - Create a new contact
- PUT /api/contacts/{id} - Update an existing contact
- DELETE /api/contacts/{id} - Delete a contact

## Testing

### Unit Tests

To run the unit tests:

1. Navigate to the project directory
2. Run `dotnet test`

### API Testing with Postman

1. Import the `ContactsAPI.postman_collection.json` file into Postman
2. Use the imported collection to test the API endpoints

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.