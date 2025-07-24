# AcademiaHub - Learning Platform

## Overview

**AcademiaHub** is a full-featured learning management system designed to streamline educational workflows for students, teachers, and administrators. The platform provides tools for managing classes, assignments, exams, and performance analysis in an efficient, organized manner.

---

## 📚 Features

### 👩‍🎓 Students
- Register and manage personal accounts.
- View, attempt, and submit assignments and exams online.
- Assignment answers are auto-saved, allowing students to continue from where they left off or modify previous responses.

### 👨‍🏫 Teachers
- Create and manage personal accounts.
- Build and assign exams and assignments to classes or student groups.
- Grade student submissions and provide feedback.
- Manage class rosters and student groups efficiently.

### 🛠️ Administrators
- Register and manage personal accounts.
- Monitor teacher and student performance across subjects and classes.
- Assign user roles (Admin, Teacher, Student).
- Manage subjects, classrooms, and question banks.

---

## ⚙️ Technical Stack

- **Backend:** ASP.NET Core Web API
- **Data Access:**
  - Repository Pattern with Generic Repository
  - Unit of Work
  - Entity Framework Core
- **Object Mapping:** AutoMapper
- **Authentication:** JWT-based authentication with role-based access control
- **Database:** SQL Server

---

## 🧱 Key Database Tables

### `Users (Identity)`
Stores authentication and role-based access info for all users (students, teachers, admins).

### `Students` / `Teachers` / `Admins`
Contain domain-specific information for each user type and link back to the shared `Users` table via foreign keys.

### `Classrooms`
Defines classes created by teachers. Each classroom can have multiple students and subjects.

### `QuestionsForm` table for Assignments and Exams with the `FormType` table to identify whether an Assignment or Quiz  
- Created by teachers and linked to classrooms.
- Store metadata such as title, due date, and associated questions.

### `QuestionBank`
Holds reusable exam/assignment questions.
- Supports multiple question types (e.g., MCQs, True/False).
- Related to the `Answers` table.

### `QBAnswers`
Represents all possible answers for a question (for MCQ or similar types).

### `FormStudentAnswers`
Ternary table capturing:
- `StudentId`, `FormId`, and `QuestionId`
- Stores each student's answer per question per exam/assignment.
- Supports partial saving of answers (used for draft/in-progress submissions).

Acts as a junction table containing:
- `StudentId`
- `FormId` (Assignment/Exam ID)
- `QuestionId`
- `StudentAnswer`

### `Form_Questions` 
Represents the relationship between forms (assignments/exams) and their included questions.
- Serves as a junction table between `QuestionsForm` and `QuestionBank`.
- Each entry links a specific question to a specific form.
Enables forms to be dynamically composed of questions from the reusable `QuestionBank`.

Supports:
- Draft mode (saving in-progress answers)
- Final submission tracking

---

## 📁 Project Architecture
AcademiaHub/
├── Controllers/
├── DTOs/
├── Mappings/ <-- AutoMapper profiles
├── Models/ <-- Entity models
├── Repositories/
│ ├── Interfaces/
│ └── Implementations/
├── Services/
├── UnitOfWork/
├── Program.cs
├── appsettings.json
└── README.md

## 🛠️ Architecture & Design Patterns

- **Repository Pattern with GenericRepository:**  
  Promotes separation of concerns and clean access to data logic.

- **Unit of Work:**  
  Coordinates the writing out of changes across multiple repositories.

- **AutoMapper:**  
  Handles the conversion between domain models and DTOs for cleaner controller logic.

---

## 🔐 Authentication & Authorization

- Role-based authorization using `[Authorize(Roles = "...")]` in controllers.
- JWT tokens are issued upon login.
- Token includes role claims to control endpoint access:
  - `Admin`
  - `Teacher`
  - `Student`

Example Token Payload:
```json
{
  "userName": "Admin@AcademiaHub.com",
  "userId": "xxx-xxx",
  "role": "Admin",
  "Id": "xxx-xxx",
  "exp": 1234567890
}
```

## ✅ Getting Started

# 🚀 How to Run the Project

## 1. Clone the Repository
git clone https://github.com/your-username/AcademiaHub.git

## 2. Configure appsettings.json

```json
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "your-app",
  "Audience": "your-app-users"
},
"ConnectionStrings": {
  "DefaultConnection": "your-database-connection-string"
}
```


## 3. Run Entity Framework Migrations
dotnet ef database update

## 4. Start the API
dotnet run


## 🔍 API Testing with Swagger
Swagger UI is available at:
https://academiahub.runasp.net/index.html

### For Testing Authorized Endpoints
#### 1. Login to retrieve your JWT token.

#### 2. Click the "Authorize" button in Swagger.

#### 3. Use the following format in the token input:
##### Bearer your-jwt-token-here

## Demo
### ERD
![ERD](/assets/E-Platform.drawio.png)

### APIs
![APIs](/assets/Apis.png)