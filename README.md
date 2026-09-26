# 🚗 Luxury Car Rental

A full-stack luxury car rental web application built with **ASP.NET Core MVC**.
The platform allows customers to explore premium vehicles and submit rental bookings, while administrators can manage cars and bookings through a dedicated admin area.

## ✨ Features

### 👤 Customer Features

* Browse available luxury cars
* View detailed car information
* Submit rental bookings
* Contact the rental service
* Responsive user interface

### 🔐 Authentication & Authorization

* User authentication with ASP.NET Identity
* Role-based authorization
* Protected admin area
* Separate customer and administrator functionality

### 🛠️ Admin Panel

* Admin dashboard
* Manage cars
* Add, edit, and delete vehicles
* Upload and manage car images
* Manage customer bookings
* View booking information

### 🚘 Car Management

Each vehicle can contain:

* Car name
* Brand
* Category
* Daily price
* Weekly price
* Model year
* Color
* Number of seats
* Transmission
* Engine
* Horsepower
* Description
* Vehicle image

## 🧰 Technologies

* **C#**
* **ASP.NET Core MVC**
* **Entity Framework Core**
* **ASP.NET Core Identity**
* **SQL Server**
* **Razor Views**
* **HTML5**
* **CSS3**
* **JavaScript**
* **Git & GitHub**

## 🏗️ Project Structure

```text
Luxury-Car-Rental/
│
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       └── Views/
│
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── ViewModels/
├── Views/
├── wwwroot/
│
├── Program.cs
├── appsettings.json
└── LuxuryCarRental.csproj
```

## 🗄️ Database

The application uses **Entity Framework Core** for database access and migrations.

The database handles:

* Cars
* Bookings
* Users
* Roles
* Application data

## 🚀 Getting Started

### Prerequisites

Make sure you have:

* .NET SDK
* Visual Studio
* SQL Server / SQL Server Express
* Git

### Installation

Clone the repository:

```bash
git clone https://github.com/ahmedkamal-31/Luxury-Car-Rental.git
```

Open the project:

```bash
cd Luxury-Car-Rental
```

Restore dependencies:

```bash
dotnet restore
```

Update the database:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

Or open `LuxuryCarRental.sln` in Visual Studio and run the project.

## 📸 Screenshots

<img width="1440" height="1520" alt="luxury_car_website_redesign" src="https://github.com/user-attachments/assets/097615da-5340-4fe7-a4de-dfe2f526ee12" />

## 🎯 Project Goal

The goal of this project was to build a realistic luxury car rental platform while applying practical **ASP.NET Core MVC** concepts including:

* MVC architecture
* Entity Framework Core
* Database migrations
* Authentication and authorization
* Role-based access control
* CRUD operations
* File/image uploads
* ViewModels
* Admin dashboard
* Form validation

## 👨‍💻 Author

**Ahmed Kamal Mohamed**

Junior .NET Developer focused on:

**C# · ASP.NET Core · MVC · Entity Framework Core · SQL Server**

GitHub:
https://github.com/ahmedkamal-31
