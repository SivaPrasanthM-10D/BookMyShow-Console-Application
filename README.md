# BookMyShow Console Application

## Overview
The **BookMyShow Console Application** is a C# console-based system that simulates an online movie ticket booking platform. It provides functionalities for users to book tickets, manage accounts, add reviews, and handle theaters and shows.

## Features
- **User Management**: Register, login, and update user details.
- **Movie Management**: View available movies and details.
- **Theater & Show Management**: Browse theaters and available showtimes.
- **Ticket Booking**: Reserve, cancel, and view booked tickets.
- **Reviews & Ratings**: Add and view reviews for movies.
- **Admin Operations**: Manage movies, theaters, and user activities.
- **Error Handling**: Custom exceptions for handling invalid inputs and operations.

## Technologies Used
- **Language**: C#
- **Framework**: .NET 8.0
- **Architecture**: Object-Oriented Programming (OOP) with Interface-based design

## Project Structure
```
BookMyShow-Console-Application/
│── BookMyShow.sln                      # Solution file
│── BookMyShow/
│   ├── BookMyShow.csproj               # Project file
│   ├── Custom Exceptions/              # Custom exception handling
│   ├── Implementations/                # Core system implementations
│   ├── Interfaces/                      # Interface definitions
│   ├── Models/                          # Data models
│   ├── bin/                             # Build output
│   ├── obj/                             # Temporary build files
```

## Installation & Running the Application

### Prerequisites
- Install **.NET 8.0 SDK**

### Steps to Run
1. **Clone the Repository**
   ```sh
   git clone https://github.com/SivaPrasanthM-10D/BookMyShow-Console-Application.git
   ```
2. **Navigate to the Project Directory**
   ```sh
   cd BookMyShow-Console-Application/BookMyShow
   ```
3. **Build the Project**
   ```sh
   dotnet build
   ```
4. **Run the Application**
   ```sh
   dotnet run
   ```

## Functionalities & Modules

### Authentication
- User Registration & Login
- Admin Authentication

### Movie Management
- View Available Movies
- Add, Update, and Delete Movies (Admin)

### Theatre & Show Management
- List Theatres and Showtimes
- Add, Update, and Delete Shows (Admin)

### Ticket Booking
- Book Tickets
- View Booking Details
- Cancel Tickets

### Review & Ratings
- Add Movie Reviews
- View Reviews and Ratings

## Contribution
Contributions are welcome! Feel free to fork the repository and submit pull requests.

## License
This project is licensed under the **MIT License**.