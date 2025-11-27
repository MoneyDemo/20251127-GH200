# TODO App - .NET 10 MVC Sample

A sample TODO application built with .NET 10 MVC demonstrating CRUD operations.

## Features

- ✅ Create, Read, Update, Delete TODO items
- ✅ Toggle completion status
- ✅ In-memory data storage for demonstration
- ✅ Bootstrap 5 responsive UI
- ✅ Form validation

## Screenshots

### TODO List
![TODO List](https://github.com/user-attachments/assets/8d0eb555-e0ae-4800-afde-cb15537455d0)

### Create TODO
![Create TODO](https://github.com/user-attachments/assets/2554244c-caac-4251-827b-8c9ff8a9cab1)

## Getting Started

### Prerequisites

- .NET 10 SDK

### Run the Application

```bash
cd TodoApp
dotnet run
```

Then open your browser and navigate to `https://localhost:5001/Todo` or `http://localhost:5000/Todo`.

## Project Structure

```
TodoApp/
├── Controllers/
│   ├── HomeController.cs
│   └── TodoController.cs      # TODO CRUD operations
├── Models/
│   ├── ErrorViewModel.cs
│   └── TodoItem.cs            # TODO data model
├── Services/
│   └── TodoService.cs         # In-memory data service
├── Views/
│   ├── Home/
│   ├── Shared/
│   └── Todo/                  # TODO views
│       ├── Index.cshtml       # List all TODOs
│       ├── Create.cshtml      # Create new TODO
│       ├── Edit.cshtml        # Edit existing TODO
│       ├── Details.cshtml     # View TODO details
│       └── Delete.cshtml      # Delete confirmation
└── Program.cs
```

## Technologies

- .NET 10
- ASP.NET Core MVC
- Bootstrap 5
- C# 13