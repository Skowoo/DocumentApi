# DocumentApi

A simple REST API created as a semester project for a backend programming class. The main functionality includes CRUD operations for a relational data set stored in an in-memory SQL database. It is built using clean architecture and the CQRS pattern, with JWT token-based authentication. The project also includes a Single Page Application (SPA) client for the API and unit and integration test projects for the API.

### Used Packages
* ASP.NET Core Identity
* ASP.NET Core Entity Framework Core
* MediatR
* FluentValidation
* Moq
* xUnit

## Deployment

To run the app, clone the repository and run the `DocumentApi.Web` project. If you want to test the API with the example client app, also run `ClientApplication`.

The app uses an in-memory SQL database for simplicity, and a built-in seeder method populates the database with sample data and two users:

* **Admin** (with full access to AdminPanel):
  * **Login:** Admin
  * **Password:** Admin

* **User:**
  * **Login:** User
  * **Password:** User
