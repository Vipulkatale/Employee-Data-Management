
```markdown
# Employee Management System

A full-stack web application for managing employees with role-based access control, built with .NET 9 Web API backend and Angular 14 frontend.

![Employee Management System](https://img.shields.io/badge/Employee-Management%20System-blue)
![Angular](https://img.shields.io/badge/Angular-14-red)
![.NET](https://img.shields.io/badge/.NET-9-purple)
![MySQL](https://img.shields.io/badge/MySQL-Database-orange)

## 🚀 Features

### 🔐 Authentication & Authorization
- **JWT-based Authentication** - Secure login/register system
- **Role-Based Access Control** - Three user roles: Admin, Manager, User
- **Protected Routes** - Automatic redirect based on user role

### 👥 Employee Management
- **Full CRUD Operations** - Create, Read, Update, Delete employees
- **Advanced Search & Filtering** - Search by name, email, position, department
- **Role-Based Permissions**:
  - **Admin**: Full access + audit logs + statistics
  - **Manager**: Employee management + statistics  
  - **User**: Read-only employee directory

### 📊 Dashboard & Analytics
- **Role-Based Dashboards** - Different interfaces for each user role
- **Real-time Statistics** - Employee counts, department stats, salary analytics
- **Interactive Charts** - Visual data representation

### 🎨 User Experience
- **Responsive Design** - Works on desktop, tablet, and mobile
- **Angular Material UI** - Modern, consistent design language
- **Real-time Updates** - Instant search and filtering
- **Loading States** - Smooth user experience during operations

## 🛠️ Tech Stack

### Frontend
- **Angular 14** - Frontend framework
- **Angular Material** - UI component library
- **TypeScript** - Programming language
- **RxJS** - State management and reactive programming
- **SCSS** - Styling

### Backend
- **.NET 9** - Web API framework
- **Entity Framework Core** - ORM
- **MySQL** - Database
- **JWT** - Authentication
- **Swagger/OpenAPI** - API documentation

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (v16 or higher)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/employee-management-system.git
cd employee-management-system
```

### 2. Backend Setup

#### Navigate to Backend Directory
```bash
cd backend
```

#### Database Configuration
1. Create a MySQL database named `employee_management`
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=employee_management;Uid=root;Pwd=yourpassword;"
  }
}
```

#### Apply Database Migrations
```bash
dotnet ef database update
```

#### Run the Backend
```bash
dotnet run
```
The API will be available at: `https://localhost:7169`

#### Access API Documentation
Visit: `https://localhost:7169/swagger`

### 3. Frontend Setup

#### Navigate to Frontend Directory
```bash
cd frontend
```

#### Install Dependencies
```bash
npm install
```

#### Configure Environment
Update `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7169/api'
};
```

#### Run the Frontend
```bash
ng serve
```
The application will be available at: `http://localhost:4200`


## 🗂️ Project Structure

```
employee-management-system/
├── 📁 backend/
│   ├── 📁 Controllers/          # API Controllers
│   ├── 📁 Models/               # Data Models
│   ├── 📁 Services/             # Business Logic
│   ├── 📁 Data/                 # Database Context
│   ├── appsettings.json         # Configuration
│   └── Program.cs               # Application Entry Point
│
├── 📁 frontend/
│   ├── 📁 src/
│   │   ├── 📁 app/
│   │   │   ├── 📁 components/   # Angular Components
│   │   │   ├── 📁 services/     # API Services
│   │   │   ├── 📁 guards/       # Route Guards
│   │   │   └── 📁 interceptors/ # HTTP Interceptors
│   │   ├── 📁 assets/           # Static Files
│   │   └── 📁 environments/     # Environment Configs
│   ├── angular.json             # Angular Configuration
│   └── package.json             # Dependencies
│
└── 📄 README.md                 # This file
```

## 🎯 Key Components

### Frontend Components
- **Login/Register** - Authentication pages
- **Admin Dashboard** - Statistics and system overview
- **Employee List** - Searchable and filterable employee table
- **Employee Form** - Add/edit employee details
- **Employee Details** - Comprehensive employee profile view
- **User Dashboard** - Read-only interface for regular users

### Backend API Endpoints

#### Authentication
- `POST /api/Auth/register` - Register new user
- `POST /api/Auth/login` - Login and get JWT token
- `GET /api/Auth/profile` - Get user profile

#### Employees
- `GET /api/Employees` - Get all employees
- `GET /api/Employees/{id}` - Get employee by ID
- `POST /api/Employees` - Create new employee
- `PUT /api/Employees/{id}` - Update employee
- `DELETE /api/Employees/{id}` - Delete employee
- `GET /api/Employees/search` - Search employees
- `GET /api/Employees/statistics` - Get dashboard statistics

#### Audit
- `GET /api/Audit` - Get audit logs (Admin only)

## 🧪 Testing

### Backend Testing
```bash
cd backend
dotnet test
```

### Frontend Testing
```bash
cd frontend
ng test
```

### E2E Testing
```bash
cd frontend
ng e2e
```

## 🚀 Deployment

### Backend Deployment
```bash
cd backend
dotnet publish -c Release
```

### Frontend Deployment
```bash
cd frontend
ng build --configuration=production
```

### Deployment Options
- **Azure App Service** - For .NET backend
- **Netlify/Vercel** - For Angular frontend
- **Docker** - Containerized deployment

## 🔧 Configuration

### Environment Variables

#### Backend (.NET)
```bash
ASPNETCORE_ENVIRONMENT=Development
JWT_SECRET=your-super-secret-key
DB_CONNECTION_STRING=Server=localhost;Database=employee_management;Uid=root;Pwd=password;
```

#### Frontend (Angular)
```typescript
// environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://your-api-domain.com/api'
};
```

## 🐛 Troubleshooting

### Common Issues

1. **CORS Errors**
   - Ensure backend CORS is configured for frontend URL
   - Check `Program.cs` for CORS policy

2. **Database Connection Issues**
   - Verify MySQL service is running
   - Check connection string in `appsettings.json`

3. **JWT Token Issues**
   - Verify token expiration settings
   - Check JWT secret configuration

4. **Build Errors**
   ```bash
   # Clear cache and reinstall
   rm -rf node_modules package-lock.json
   npm install
   ```

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow Angular style guide
- Write unit tests for new features
- Update documentation accordingly
- Ensure responsive design works on all screen sizes

## 📝 API Documentation

For detailed API documentation, run the backend and visit:
`https://localhost:7169/swagger`

## 🗃️ Database Schema

### Users Table
- `Id` (Primary Key)
- `Username` (Unique)
- `Email` (Unique)
- `PasswordHash`
- `Role` (Admin/Manager/User)
- `CreatedAt`

### Employees Table
- `Id` (Primary Key)
- `EmployeeCode` (Unique)
- `FirstName`
- `LastName`
- `Email`
- `Phone`
- `Position`
- `Department`
- `Salary`
- `HireDate`
- `CreatedBy`
- `Timestamps`

### AuditLogs Table
- `Id` (Primary Key)
- `TableName`
- `RecordId`
- `Action`
- `OldValues`
- `NewValues`
- `ChangedBy`
- `Timestamp`

## 🔒 Security Features

- Password hashing with BCrypt
- JWT token expiration
- Role-based endpoint protection
- CORS configuration
- Input validation and sanitization
- Audit logging for all operations

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

## 🙏 Acknowledgments

- Angular Team for the amazing framework
- .NET Team for the robust backend framework
- Material Design for the UI components
- All contributors who helped shape this project

---

<div align="center">

**⭐ Don't forget to star this repository if you find it helpful!**

</div>
```
