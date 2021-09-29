# Lottery

## Summary ##

The Lottery API provides users with the following functionality:
- The ability to create a new lottery ticket with N lines
- The ability to get an individual ticket
- The ability to get all tickets
- The ability to update an existing ticket
- The ability to retrieve the status of a ticket

## Test Strategy ##

### Unit Tests ###

Unit Tests will be added at a Controller, Service, and Repository Level.

These include:

**Controller Tests**

- Ticket Controller Tests
    - These tests will check response codes and response messages for POST Ticket, PUT Ticket, GET Ticket by Id, and GET All Tickets

- Status Controller Tests
    - These tests will check response code and response message for PUT Status Endpoint

**Service Tests**
- Ticket Service Tests
    - These tests will check success and failure on the Create, Update, Get and GetAll methods on the Ticket Service

- Status Service Tests
    - These tests will check the Lottery Rules that the Status Service handles

**Repository Tests**
- Ticket Repository Tests
    - These tests will check the Save, Update, Get, GetAll, and UpdateCheckedStatus.

### End To End Tests ###

The End to End Testing will be achieved using the provided Postman collection.

The following outlines the scenarios to be covered - Where possible, the Response Code, Response Message and Data values will be validated.

Scenarios covered:

- POST Ticket - Valid Request - Returns 200/OK
- POST Ticket - Invalid Request - Returns 400/Bad Request
- POST Ticket - Invalid Number Of Lines - Returns 400/Bad Request
- GET Ticket - Valid Id - Returns Ticket - Returns 200/OK
- GET Ticket - Does Not Exist - Returns 404/NotFound
- GET Ticket - No Id Provided - Returns All Tickets - 200/OK
- PUT Ticket - Valid Request - 200/OK
- PUT Ticket - Invalid Request - Returns 400/Bad Request
- PUT Ticket - Invalid Number Of Lines - Returns 400/Bad Request
- PUT Ticket - Creates If Ticket Does Not Exist - 200/OK
- PUT Ticket - Ticket Already Checked - Returns 400/Bad Request
- PUT Status - Ticket Exists - Returns 200/OK
- PUT Status - Ticket Does Not Exist - Returns 404/Not Found 