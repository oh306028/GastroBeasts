# GastroBeast API

GastroBeast is a RESTful API designed to manage restaurant-related data, including addresses, categories, reviews, and user accounts. It provides comprehensive endpoints for creating, reading, updating, and deleting restaurant information.

- [API Endpoints](#api-endpoints)
  - [Address](#address)
  - [Category](#category)
  - [Restaurant](#restaurant)
  - [Review](#review)
  - [User](#user)
- [Authentication](#authentication)
- [Error Handling](#error-handling)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)


## API Endpoints

### Address

- **GET /api/restaurants/{restaurantId}/address**
  - Retrieves the address of a specific restaurant by its ID.

- **POST /api/restaurants/{restaurantId}/address**
  - Adds or updates the address for a specific restaurant.

### Category

- **POST /api/restaurants/categories**
  - Creates a new restaurant category.

- **GET /api/restaurants/categories**
  - Retrieves a list of all restaurant categories.

- **GET /api/restaurants/{restaurantId}/categories**
  - Retrieves the categories associated with a specific restaurant.

- **POST /api/restaurants/{restaurantId}/categories**
  - Adds categories to a specific restaurant.

### Restaurant

- **GET /api/restaurants**
  - Retrieves a list of all restaurants.

- **POST /api/restaurants**
  - Creates a new restaurant.

- **GET /api/restaurants/all**
  - Retrieves detailed information about all restaurants.

- **GET /api/restaurants/{id}**
  - Retrieves information about a specific restaurant by its ID.

- **DELETE /api/restaurants/{restaurantId}**
  - Deletes a specific restaurant by its ID.

- **PUT /api/restaurants/{restaurantId}**
  - Updates the details of a specific restaurant.

### Review

- **POST /api/restaurants/{restaurantId}/reviews**
  - Adds a new review to a specific restaurant.

- **GET /api/restaurants/{restaurantId}/reviews**
  - Retrieves reviews for a specific restaurant.

- **DELETE /api/restaurants/{restaurantId}/reviews/{reviewId}/delete**
  - Deletes a specific review by its ID.

- **PUT /api/restaurants/{restaurantId}/reviews/{reviewId}/update**
  - Updates a specific review by its ID.

### User

- **POST /api/account/register**
  - Registers a new user.

- **POST /api/account/login**
  - Logs in an existing user and returns a JWT token.

## Authentication

The GastroBeast API uses JWT (JSON Web Tokens) for authentication. Users must include a valid JWT token in the `Authorization` header when making requests to protected endpoints.

### Obtaining a Token

To obtain a JWT token, send a POST request to `/api/account/login` with valid user credentials. The response will include the token that can be used for authenticated requests.


## Error Handling

The API returns standard HTTP status codes to indicate the success or failure of an API request. Common status codes include:

- `200 OK` - The request was successful.
- `201 Created` - The resource was successfully created.
- `400 Bad Request` - The request was invalid or cannot be otherwise served.
- `401 Unauthorized` - Authentication failed or user does not have permissions for the requested operation.
- `404 Not Found` - The requested resource could not be found.
- `500 Internal Server Error` - An error occurred on the server.

## Testing

For testing purposes, consider using tools such as Postman to send requests to the API endpoints and verify responses. Automated tests can also be set up using your preferred testing framework.

## Contributing

Contributions to the GastroBeast API are welcome! If you'd like to contribute, please fork the repository, make your changes, and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

