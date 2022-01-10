# Overview
This exercise is intentionally left open ended.  Within you will find a skeleton code base and a JSON file.  This JSON file is to be used as your datastore.

# Requirements
 - API listing all customers
 - API retrieving a customer
 - API adding a customer 
 - API updating a customer
 - API deleting a customer
 - API should preserve state
 - Unit tests
 - Use .NET Core 3.1 or NET 5+

# Expectations
Implement the above listed requirements in a manner you see fitting. Demonstrate design and implementation aspects you feel are important in a production ready software project.

# Publish
Send us a public link to your solution.  If you don't want to make it public, feel free to just send us a ZIP file.

# PLEASE READ THIS
# Comments on code delivery
- Work done
	- Used Controller-Service-Repository layer to segregate the responsibility of each component.
		- Controller will be tied to the web technology we use i.e., webapi or graphql or function and acts as a thin layer for interaction
		- Service layer is where all the business logic reside. Currently, there is none so it is barebone in nature. But in real use case, this is where all the domain knowledge will be isolated.
		- Repository layer is only concerned with fetching the data. The datastore could be any db, file, blobs or other webservices. Main purpose is to isolate the data retrieval from the business domain.
	- Utilized Global exception handler to catch any uncaught exceptions.
	- ExceptionHandling vs ResultObject
		- Age old debate is to use either Result object or use Exception handling for altering the "unhappy" path. This is left open-ended in this solution.
	- Each project will know what it needs to register. Startup file in API should not bother about what objects to register in Repository or Service layer.
	- Using Microsoft Logging extension so that it can write to multiple sources instead of Serilog. The interface is less likely to change and it will be supported for a longer time.
	- Tests are broken into 3 types
		- Unit tests for API and Service projects
		- Integration tests for Repository project as it access network or physical files.
		- End-End project (which is not included here) which creates SUT (System Under Test) and runs the entire application.
	- Model/DTO
		- Currently the Models project represents the domain model.
		- for a large solution, it is recommended to use a GroceryStore.DataModels and GroveryStore.ApiModels prject to represent different DTO
		- Utilizing Automapper and its succinct tests to make sure that the mapping can be done efficiently.
- What is left out?
	- Not all unit tests are written for branch coverage. It can definitely be expanded. Similar case for "unhappy path".
		- some examples are included for "unhappy path"
	- The documents are written using Swashbuckle especially the API project. Redoc should also work based on XML comment structure.
		- Multiple example for Swashbuckle can use a little more work using Filters.
	- Validatations are left out. Fluent Validation can be used to perform different validation for each layer.
