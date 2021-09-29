# Lottery API

The Lottery API provides users with the following functionality:
- The ability to create a new lottery ticket with N lines
- The ability to get an individual ticket
- The ability to get all tickets
- The ability to update an existing ticket with additional N lines
- The ability to retrieve the status of a ticket

## Pre-requisites ##

[.NET 5 SDK](https://dotnet.microsoft.com/download) is required to run this project.

### How to use ###

To run this project, navigate to the LotteryApi folder and run:

    dotnet run


To run all the unit tests for this project, navigate to LotteryApiTests folder and run:

    dotnet test

## API endpoints ##

The API endpoints available:

    POST Ticket:    https://localhost:5001/ticket
    PUT Ticket:     https://localhost:5001/ticket/{id}
    GET Ticket:     https://localhost:5001/ticket/{id}
    GET Tickets:    https://localhost:5001/ticket
    GET Status:     https://localhost:5001/status/{id}

## Swagger ##

Swagger is included with this project. 

Once the application is running, this can be accessed at:

    https://localhost:5001/swagger/index.html

## Postman Collection ##

A Postman Collection is included with this project.

***How to run the Lottery Postman Collection***

- Ensure the LotteryApi project is running locally on https://localhost:5001
- Download the Postman Collection & Import the collection into Postman
- Once the 'Lottery Collection' has been imported, you can choose the 'Run Collection' option
    - This will present you with a preview screen & you can choose 'Run Lottery Collection' to start the test run.
- Each test can also be run individually - You can see the outcome in the 'Test Results' of the response pane

## Test plan ##

The Test Plan can be found in the TestPlans Folder provided. It contains an outline of the test strategy taken, and includes a high-level overview of the unit test project (LotteryApiTests) and the end-to-end tests defined in the included Lottery Postman Collection.

Story test strategies available:

    Lottery.md