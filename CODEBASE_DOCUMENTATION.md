# MoneyTracking - Codebase Documentation

## 📋 Project Overview

**Purpose**: MoneyTracking is a personal finance management application written in C#/.NET 10.0. It is designed to help users track their financial accounts, transactions, categories, and budgets.

**Language/Platform**: 
- C# with .NET 10.0
- Cross-platform support (standard .NET SDK)

**Development Status**: Early development stage - a work-in-progress library project with core domain models implemented but no presentation layer or database persistence yet.

---

## 🏗️ Architecture & Technology Stack

### Architecture Pattern
Clean Architecture / Domain-Driven Design (DDD) with clear separation of concerns:

- **Domain Layer** (Domain/) - Core business entities, value objects, and domain logic
- **Features Layer** (Features/) - Application services and use cases
- **Infrastructure Layer** (Infra/) - Data access interfaces (no implementation yet)
- **Tests Layer** (Tests/) - Unit and integration tests

### Key Technologies & Frameworks
- **.NET 10.0** - Target framework (using Microsoft.NET.Sdk)
- **xUnit 2.9.3** - Testing framework
- **coverlet.collector 6.0.4** - Code coverage tool
- **Microsoft.NET.Test.Sdk 17.14.1** - Testing SDK

### Build Tool
Visual Studio 2022 (Solution file: `MoneyTracking.sln`)

### Notable Absences
- No web framework (ASP.NET Core not implemented)
- No ORM (Entity Framework, Dapper, etc. not yet added)
- No database persistence layer
- No Docker or containerization
- No API or UI layer
- No README or documentation files

---

## 📁 Project Structure

```
MoneyTracking/
├── Domain/                          # Core domain layer
│   ├── Account/
│   │   ├── Account.cs               # Account entity
│   │   ├── AccountName.cs           # Value object
│   │   ├── AccountType.cs           # Value object (enum: Checking, Savings)
│   │   └── Balance.cs               # Value object
│   ├── Category/
│   │   ├── Category.cs              # Category entity
│   │   ├── CategoryName.cs          # Value object
│   │   ├── CategoryType.cs          # Value object (enum: Income, Expense)
│   │   └── MonthlyBudget.cs         # Entity (budget per month/year)
│   ├── Common/
│   │   ├── Entity.cs                # Base entity class
│   │   ├── Email.cs                 # Value object with validation
│   │   ├── Error/                   # Error handling
│   │   │   ├── Error.cs
│   │   │   └── ErrorType.cs         # Enum: Failure, NotFound, Validation, Conflict, etc.
│   │   ├── Result/                  # Result pattern
│   │   │   ├── Result.cs
│   │   │   └── ResultT.cs           # Generic result type
│   │   └── MonthYear.cs             # Value object for month/year operations
│   ├── Transaction/
│   │   ├── Transaction.cs           # Transaction entity
│   │   ├── TransactionAmount.cs     # Value object
│   │   ├── TransactionDescription.cs # Value object
│   │   └── TransactionType.cs       # Value object (enum: Income, Expense)
│   ├── User/
│   │   ├── User.cs                  # User entity
│   │   └── UserName.cs              # Value object (UserFullName)
│   ├── GlobalUsing.cs               # Global using directives
│   └── Domain.csproj
├── Features/                        # Application services layer
│   ├── Account/
│   │   ├── AccountFeature.cs        # Account operations
│   │   └── Add/
│   │       └── Add.cs               # Add account feature
│   ├── Common/
│   │   ├── Error/                   # Duplicate error handling (same as Domain)
│   │   │   ├── Error.cs
│   │   │   └── ErrorType.cs
│   │   └── Result/                  # Duplicate result pattern
│   │       ├── Result.cs
│   │       └── ResultT.cs
│   ├── User/
│   │   ├── UserFeature.cs           # User operations
│   │   └── Create/
│   │       └── Create.cs            # Create user feature
│   ├── GlobalUsings.cs
│   └── Features.csproj
├── Infra/                           # Infrastructure layer
│   ├── Repositories/
│   │   ├── IAccountRepository.cs    # Account repository interface
│   │   └── IUserRepository.cs       # User repository interface
│   ├── GlobalUsing.cs
│   └── Infra.csproj
├── Tests/                           # Testing layer
│   ├── Features/
│   │   └── UserFeatures/
│   │       └── UserFeaturesTests.cs # (Outdated test - references non-existent methods)
│   ├── UnitTest1.cs                 # Placeholder test
│   └── Tests.csproj
├── MoneyTracking.sln                # Visual Studio solution
└── .gitignore                       # Standard .NET gitignore
```

### Statistics
- Total C# files: 36
- Domain: 22 files
- Features: 9 files
- Infrastructure: 3 files
- Tests: 2 files
- Total lines of code: ~758

---

## 🗄️ Data Models & Entities

### Core Entities (all inherit from `Entity` base class):

#### 1. Entity (Base Class)
- Properties: `Id` (Guid), `CreatedAt` (DateTimeOffset), `UpdatedAt` (DateTimeOffset?)
- Implements `IEquatable<Entity>` with equality based on Id
- Protected method `UpdateTimestamp()` for tracking changes

#### 2. User
- Properties:
  - `Name` (UserFullName - value object)
  - `Email` (Email - value object)
  - `Accounts` (IReadOnlySet<Account>)
  - `Categories` (IReadOnlySet<Category>)
- Static factory: `User.Create(string name, string email)`
- Methods: `UpdateName()`, `UpdateEmail()`, `AddCategory()`, `RemoveCategory()`, `AddAccount()`
- Validations: Prevents duplicate accounts/categories by name and type

#### 3. Account
- Properties:
  - `Name` (AccountName - value object)
  - `Type` (AccountType - enum: Checking, Savings)
  - `Balance` (Balance - value object)
  - `Transactions` (IReadOnlySet<Transaction>?)
  - `IsActive` (bool)
- Static factory: `Account.Create(string name, AccountTypeEnum type, decimal balance, HashSet<Transaction>? transactions)`
- Methods: `UpdateName()`, `UpdateType()`, `Credit()`, `Debit()`
- Balance automatically updates on credit/debit operations

#### 4. Transaction
- Properties:
  - `Amount` (TransactionAmount - value object)
  - `Type` (TransactionType - enum: Income, Expense)
  - `Date` (DateTimeOffset)
  - `Description` (TransactionDescription - optional)
  - `Account` (Account entity)
  - `Category` (Category entity)
- Static factory: `Transaction.Create(TransactionAmount amount, TransactionType type, DateTimeOffset date, TransactionDescription? description, Account account, Category category)`
- Validations: Income must be positive, Expense must be negative

#### 5. Category
- Properties:
  - `Name` (CategoryName - value object)
  - `Type` (CategoryType - enum: Income, Expense)
  - `IsActive` (bool)
  - `MonthlyBudget` (MonthlyBudget - optional)
- Static factory: `Category.Create(CategoryName name, CategoryType type)`
- Methods: `UpdateName()`, `UpdateType()`, `SetMonthlyBudget()`, `RemoveMonthlyBudget()`
- TODOs: Icon and Color properties not yet implemented

#### 6. MonthlyBudget (Entity)
- Properties:
  - `Amount` (decimal)
  - `MonthYear` (MonthYear - value object)
  - `Category` (Category entity)
- Static factory: `MonthlyBudget.Create(decimal amount, MonthYear monthYear, Category category)`

### Value Objects (immutable record types):

- **UserFullName** - Splits into FirstName/LastName with capitalization
- **Email** - Regex validation, trimmed and lowercased
- **AccountName** - 3-50 characters
- **AccountType** - Predefined instances for Checking/Savings
- **Balance** - Supports Credit/Debit operations with operator overloading
- **TransactionAmount** - Simple wrapper
- **TransactionDescription** - Simple wrapper
- **TransactionType** - Predefined instances for Income/Expense
- **CategoryName** - 3-50 characters
- **CategoryType** - Predefined instances for Income/Expense
- **MonthYear** - Comprehensive date operations (Current, AddMonths, Next, Previous, etc.)

### Common Patterns:

#### Result Pattern
- `Result` - Success/Failure without value
- `ResultT<TValue>` - Success with value / Failure with error(s)
- Implicit conversions for easy use
- JSON serialization support

#### Error Pattern
- Static factory methods: `Failure()`, `NotFound()`, `Validation()`, `Conflict()`, `AccessUnAuthorized()`, `AccessForbidden()`
- Properties: `Code`, `Description`, `ErrorType`

---

## ⚙️ Application Services (Features Layer)

Currently implemented features:

### 1. UserFeature
- Dependencies: `IUserRepository`
- Implemented Method:
  - `CreateUserAsync(CreateUserRequest request)` - Creates a new user
  - Validations: Checks if user with email already exists
  - Returns: `ResultT<CreateUserResult>` with UserId

### 2. AccountFeature
- Dependencies: `IUserRepository`, `IAccountRepository`
- Implemented Method:
  - `AddAccountAsync(AddAccountRequest request)` - Adds account to user
  - Validations: Checks if user exists
  - Returns: `ResultT<AddAccounResult>` with AccountId

**Note**: The test file references `AddUserAsync` and `AddUserRequest` which don't exist - the test is outdated.

---

## 🔌 Data Access (Infrastructure Layer)

Only repository **interfaces** are defined (no implementation):

### 1. IUserRepository
- `GetByIdAsync(Guid id)` - Get user by Id
- `GetByEmailAsync(Email email)` - Get user by email
- `AddAsync(User user)` - Add new user

### 2. IAccountRepository
- `GetByIdAsync(Guid id)` - Get account by Id
- `AddAsync(Account account, Guid userId)` - Add account to user

**Status**: No concrete implementation, ORM, or database configuration.

---

## 🧪 Testing

### Test Framework
xUnit

### Test Coverage
- Total test files: 2 (plus one placeholder)
- Tests directory structure mirrors Features directory
- Using Moq for mocking repositories

### Test Files
1. `UnitTest1.cs` - Empty placeholder
2. `Tests/Features/UserFeatures/UserFeaturesTests.cs` - Outdated, references non-existent `AddUserAsync` method

### Testing Tools
- `coverlet.collector 6.0.4` - Code coverage collection
- `xunit 2.9.3` - Test framework
- `xunit.runner.visualstudio 3.1.4` - Visual Studio test runner

**Note**: The test suite is incomplete and needs updates to match actual feature implementations.

---

## 🔨 Build & Setup Process

### Build System
MSBuild (via .NET SDK)

### Configuration Files
- `MoneyTracking.sln` - Solution file with 4 projects
- `*.csproj` files - Standard .NET project files

### Project Dependencies
- Domain → No external dependencies
- Features → Domain, Infra
- Infra → Domain
- Tests → Domain, Features, Infra, xUnit, coverlet

### Build Commands
```bash
dotnet build              # Build solution
dotnet test                # Run tests
dotnet test --collect:"XPlat Code Coverage"  # Run with coverage
```

**Environment**: No environment-specific configuration files found (no appsettings.json, no .env file).

---

## 🎨 Key Design Patterns & Best Practices

### Patterns Used
1. **Domain-Driven Design (DDD)** - Rich domain models, value objects, aggregates
2. **Clean Architecture** - Separation of concerns, dependency inversion
3. **Result Pattern** - For error handling without exceptions
4. **Repository Pattern** - For data access abstraction
5. **Factory Pattern** - Static `Create()` methods on entities
6. **Record Types** - For immutable value objects
7. **Partial Classes** - Features split across multiple files

### Best Practices
- Encapsulation with private setters
- Validation in value object constructors
- Identity-based equality for entities
- Immutable value objects (record types)
- Null reference types enabled (`<Nullable>enable</Nullable>`)
- Implicit usings for cleaner code
- Global usings for common imports

**Language Version**: C# 12+ features (record types, file-scoped namespaces, implicit usings, etc.)

---

## 📊 Code Quality & Maintenance

### Strengths
- Clean architecture with clear separation
- Comprehensive domain modeling
- Consistent use of Result pattern
- Value objects with validation
- Well-structured entity relationships
- Modern C# features utilized

### Areas for Improvement
1. **No README or documentation** - Project lacks documentation
2. **Infrastructure layer incomplete** - No repository implementations
3. **Test coverage minimal** - Only one placeholder test
4. **Outdated tests** - Test references non-existent methods
5. **No presentation layer** - No API or UI to use the features
6. **Duplicate code** - Result and Error classes duplicated in Domain and Features
7. **Missing features** - No Transaction or Category features implemented yet
8. **TODOs** - Category entity has TODO for Icon and Color properties

---

## 📜 Git History

### Branches
`master` (main branch)

### Recent Commits (all from January 2026)
1. `9fea0d5` - Removal of properties not part of object context, addition of ValueObjects
2. `891cb22` - Refactored namespaces, added MonthlyBudget entity
3. `e30d4a1` - Refactored collections to HashSet, added Transaction entity
4. `e715817` - Added Category entity and ID-based equality operator
5. `e87650b` - Initial commit

**Development Pace**: Active development in early January 2026

---

## 🚀 Entry Points & Application Flow

### Current State
This is a **library project**, not an executable application. There is no `Program.cs` or `Main()` method.

### How It Would Work (when complete)
1. Presentation layer (API/Console/UI) would call Feature methods
2. Features use Repositories to persist/load data
3. Features execute domain logic through entities
4. Results returned using Result pattern for success/failure handling

### Example Flow
```
API → Feature.CreateUserAsync() → User.Create() → IUserRepository.AddAsync() → ResultT<CreateUserResult>
```

---

## 🔗 Dependency Graph

```
Tests
  ├── Domain
  ├── Features
  │   ├── Domain
  │   └── Infra
  │       └── Domain
  └── Infra
      └── Domain

Features
  ├── Domain
  └── Infra
      └── Domain

Infra
  └── Domain

Domain (no dependencies)
```

---

## 📌 Summary

MoneyTracking is a well-architected, domain-driven C# application for personal finance management. It demonstrates excellent practices in domain modeling, value object usage, and clean architecture principles. However, it's in early development with only the core domain layer and basic user/account features implemented. The project lacks infrastructure implementation (database, ORM), presentation layer, comprehensive tests, and documentation. The codebase is production-ready in terms of architecture but needs significant work before it can be deployed as a usable application.

### Key Strengths
- Modern .NET 10.0 with latest C# features
- Clean DDD architecture
- Rich domain modeling with proper encapsulation
- Consistent Result pattern for error handling

### Next Steps for Completion
1. Implement repository layer with ORM (Entity Framework Core recommended)
2. Add presentation layer (Web API or Blazor UI)
3. Implement remaining features (Transactions, Categories)
4. Write comprehensive tests
5. Add documentation (README, API docs)
6. Complete Category icon/color features
7. Add validation and error handling improvements
8. Add logging and monitoring
9. Consider adding authentication/authorization
10. Add configuration management (appsettings, environment variables)
