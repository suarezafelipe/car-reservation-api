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

This application provides a simple and efficient way to reserve cars for upcoming rides. Users can manage the cars in the system, represented by their make, model, and unique identifier following the pattern "C<number>". Reservations can be made up to 24 hours in advance, with a maximum duration of 2 hours. The system will find an available car, store the reservation if possible, and return a response with the details.

## Controllers

The application has two main controllers:

1. `CarController`: Manages car operations such as adding, updating, retrieving, and deleting cars.
2. `ReservationController`: Handles reservation-related operations such as listing all reservations and creating new reservations.

## Architecture

This project follows a clean architecture approach, with a business layer that doesn't have dependencies on other projects. It ensures the separation of concerns, making the codebase more maintainable and scalable.

## Testing

The application has an extensive suite of unit tests, providing a code coverage of 97% across the solution. This helps ensure the reliability and stability of the application.

## Documentation

The API is fully documented and can be accessed via the Swagger UI when running the application locally. It allows users to explore and interact with the API, making it easier to understand its capabilities and usage.

## Docker

A Dockerfile is included in the project, allowing you to build and run the application using Docker. The generated Docker image is tested and confirmed to be working.

## Getting Started

To get started with the Car Reservation API, simply clone the repository and follow the instructions to set up and run the application. If you have any questions or need assistance, feel free to reach out or consult the documentation.

Happy reserving!
