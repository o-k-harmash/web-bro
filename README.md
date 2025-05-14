# WebBro - Learning Path Platform

## Welcome! 👋

WebBro is a learning path platform designed to guide users through structured learning experiences. This project is inspired by **Frontend Mentor** challenges but tailored for a full-stack learning experience.

---

## Features

- **Learning Paths**: Users can explore and follow structured learning paths.
- **Step Navigation**: Each learning path consists of steps (articles or challenges) with clear navigation.
- **Progress Tracking**: Users can track their progress through each step and stage.
- **Dynamic Step Stages**: Challenges are divided into stages, and progress is updated dynamically.
- **Role-Based Access**: Future-proofed for user role validation and permissions.

---

## Business Scenarios

### 1. **Dashboard**
- **URI**: `/learning-paths`
- **Description**: Displays a list of all learning paths with progress previews.

### 2. **Start Learning Path**
- **URI**: `/learning-paths/{learningPathId}/start`
- **Description**: Initializes a learning path and redirects to the first step.

### 3. **Continue Learning**
- **URI**: `/learning-paths/{learningPathId}/continue`
- **Description**: Redirects the user to the next unfinished step in the learning path.

### 4. **Open Step**
- **URI**: `/learning-paths/{learningPathId}/steps/{stepId}/open`
- **Description**: Opens a specific step (article or challenge) and determines its stage.

### 5. **Complete Step**
- **URI**: `/learning-paths/{learningPathId}/steps/{stepId}/complete`
- **Description**: Marks the current step as completed and redirects to the next step.

---

## Technical Details

### Backend
- **Framework**: ASP.NET Core
- **Database**: Entity Framework Core with SQL Server
- **Architecture**: Clean Architecture with layered services
- **Key Services**:
  - `LearningPathService`: Handles business logic for learning paths.
  - `NavigationService`: Determines step navigation and stages.
  - `ProgressService`: Tracks and updates user progress.

### Frontend
- **Framework**: Razor Views
- **Styling**: Tailwind CSS (or similar CSS framework)
- **Dynamic Navigation**: Links are dynamically generated based on step type and stage.

---

## How to Run

1. Clone the repository:
```bash
    git clone https://github.com/your-repo/webbro.git
    cd webbro
```
2. Set up the database:
```bash
    dotnet ef migrations add InitialMigration --startup-project ./src/WebBro --project ./src/DataLayer/ --output-dir Migrations
    dotnet ef database update --startup-project ./src/WebBro --project ./src/DataLayer/
```
3. Run the application:
```bash
   dotnet run --project ./src/WebBro
```
4. Open your browser and navigate to:
```bash
    http://localhost:5000/learning-paths
```

---

## Future Enhancements
- **User Authentication**: Add role-based access control for steps and paths.
- **Advanced Analytics**: Provide detailed progress analytics for users.
- **Customizable Paths**: Allow users to create and share their own learning paths.
- **API Integration**: Expose RESTful APIs for external integrations.

---

## Feedback
We'd love to hear your thoughts! If you have any feedback or suggestions, feel free to open an issue or contribute to the project.

---

## Happy Learning! 🚀 ```