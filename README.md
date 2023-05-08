# Car Reservation API

[![.NET](https://github.com/suarezafelipe/car-reservation-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/suarezafelipe/car-reservation-api/actions/workflows/dotnet.yml)

The Car Reservation API allows users to manage and reserve cars for upcoming rides.

## Features

- Add, update, and remove cars
- List all cars and upcoming reservations
- Reserve a car for a specific time and duration
- Clean architecture approach with a business layer free from external dependencies
- Extensive use of unit tests with a 97% code coverage
- Full API documentation accessible via Swagger UI
- Docker support with a working Docker image

## Overview

This application provides a simple and efficient way to reserve cars for upcoming rides. Users can manage the cars in the system, represented by their make, model, and unique identifier following the pattern "C-number-". Reservations can be made up to 24 hours in advance, with a maximum duration of 2 hours. The system will find an available car, store the reservation if possible, and return a response with the details.

## API Versioning

This API uses versioning in its URLs. Make sure to include the version number (e.g., `v1`) when making requests to the endpoints. For example:
`https://localhost:7023/api/v1/Car`

## Controllers

The application has two main controllers:

1. `CarController`: Manages car operations such as adding, updating, retrieving, and deleting cars.
2. `ReservationController`: Handles reservation-related operations such as listing all reservations and creating new reservations.

## Architecture

This project follows a clean architecture approach, with a business layer that doesn't have dependencies on other projects. It ensures the separation of concerns, making the codebase more maintainable and scalable.

![image](https://user-images.githubusercontent.com/26448135/236864392-3d83291b-114f-4c78-b9c4-222b9e1a0fda.png)


## Testing

The application has an extensive suite of unit tests, providing a code coverage of 97% across the solution. This helps ensure the reliability and stability of the application.

![image](https://user-images.githubusercontent.com/26448135/236862432-667df7cf-a290-4f94-a32f-5226e44e1cbe.png)


## Documentation

The API is fully documented and can be accessed via the Swagger UI when running the application locally. It allows users to explore and interact with the API, making it easier to understand its capabilities and usage.

![image](https://user-images.githubusercontent.com/26448135/236863448-918fe9fd-912f-43b7-bdca-bc653368fd3a.png)


## Docker

A Dockerfile is included in the project, allowing you to build and run the application using Docker. The generated Docker image is tested and confirmed to be working.

![image](https://user-images.githubusercontent.com/26448135/236862801-6b022b18-4655-4335-8108-f1c8ed829487.png)

## Logging and Error Handling Middleware

The Car Reservation API includes a custom middleware for logging and error handling. This middleware ensures proper logging of all incoming requests and their corresponding responses, including any unhandled exceptions that may occur during the request processing.

For each request, a unique correlation ID is generated and added to both the request and response headers. This correlation ID is used for tracing purposes, making it easier to track and correlate logs for individual requests.

The middleware logs the following information for each request:

- HTTP method
- Request path
- Correlation ID
- Response status code

In case of an unhandled exception, the middleware captures the error and logs additional information, such as the request body and the exception details. It then returns a `500 Internal Server Error` response to the client, along with a generic error message and the correlation ID for further investigation.

This middleware ensures a consistent approach to logging and error handling throughout the API, improving maintainability and facilitating easier troubleshooting.


## Getting Started

To get started with the Car Reservation API, simply clone the repository and follow the instructions to set up and run the application. If you have any questions or need assistance, feel free to reach out or consult the documentation.

Happy reserving!
