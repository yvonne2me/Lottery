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

- Controller Tests
    - Expect 200/OK for valid POST Ticket request
    - Expect 400/BadRequest for Null Ticket request
    - Expect Error Message when exception thrown during POST Ticket
    - Expect GET Ticket Request to return individual Ticket when it exists
    - Expect GET Ticket returns 404/NotFound when individual Ticket does not exist
    - Expect GET to returns all Tickets that exist
    - Expect GET to return 404/NotFound when Tickets do not exist
    - Expect PUT Ticket to update existing Ticket
    - Expect PUT Ticket to return 404/NotFound when Ticket does not exist
    - Expect PUT Ticket to return 400/BadRequest for Null Ticket request

### End To End Tests ###

The End to End Testing will be achieved using the provided Postman collection.

Scenarios covered:

- POST Ticket - Valid Request - 200/OK
- POST Ticket - Invalid Request - 400/Bad Request
- GET Ticket - Valid Id - Returns Ticket - 200/OK
- GET Ticket - Does Not Exist - Returns 404/NotFound
- GET Ticket - No Id Provided - Returns All Tickets - 200/OK
- GET Ticket - No Id Provided and No Ticket Exists - Returns 404/NotFound
- PUT Ticket - Valid Request - 200/OK
- PUT Ticket - Invalid Request - 400/BadRequest
- PUT Ticket - Ticket does not exist - 404/NotFound **TODO - Review this: Should it create if it does not exist**