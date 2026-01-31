# AGENTS.md

## Build/Test Commands

### Build
```bash
dotnet build                  # Build entire solution
dotnet build Domain/Domain.csproj          # Build specific project
dotnet clean                   # Clean build artifacts
dotnet restore                 # Restore NuGet packages
```

### Test
```bash
dotnet test                   # Run all tests
dotnet test --filter "FullyQualifiedName~ClassName"              # Run specific test class
dotnet test --filter "FullyQualifiedName~ClassName.MethodName"   # Run specific test method
dotnet test --filter "DisplayName~Test Description"              # Run test by display name
dotnet test --collect:"XPlat Code Coverage"                      # Run with coverage
dotnet test --logger "console;verbosity=detailed"                 # Detailed test output
dotnet test Tests/Tests.csproj                                   # Run tests for specific project
```

## Code Style Guidelines

### General
- **Language**: C# 12+ with .NET 10.0
- **Architecture**: Modular Monolith Architecture  / Domain-Driven Design (DDD)
- **Null-safety**: Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- **Usings**: Implicit usings enabled, global usings in GlobalUsing.cs
- **Namespaces**: File-scoped, match directory structure

### Imports
- Global usings defined in `Domain/GlobalUsing.cs` for common domain imports
- Global usings defined in `Features/GlobalUsings.cs` for feature imports
- Additional imports as needed at top of files
- Pattern: `global using Domain.Common; global using Domain.Entities; global using Domain.ValueObjects;`

### Naming Conventions
- **Classes**: PascalCase (e.g., `UserFeature`, `Account`)
- **Interfaces**: IPascalCase (e.g., `IUserRepository`)
- **Methods**: PascalCase, async methods end with `Async` (e.g., `CreateUserAsync`, `GetByIdAsync`)
- **Properties**: PascalCase (e.g., `Name`, `Balance`)
- **Private fields**: _camelCase with backing field pattern (e.g., `_name`, `_accounts`)
- **Constants**: PascalCase in static classes (e.g., `User.NameUnchanged`)
- **Value Objects**: PascalCase, often as record types (e.g., `UserFullName`, `Email`)
- **Error constants**: PascalCase in *Errors.cs files (e.g., `UserFullName.EmptyOrNull`)

### Types & Patterns

#### Entities
- Inherit from `Entity` base class (Id, CreatedAt, UpdatedAt)
- Static factory method `Create()` returns `ResultT<TEntity>`
- Internal constructor with Guid id parameter
- Private setters with backing fields for mutable properties
- Expose collections via `IReadOnlySet<T>` backed by `HashSet<T>`
- Methods return `ResultT<TEntity>` for chaining
- Sealed classes (no inheritance)

#### Value Objects
- Immutable record types
- Private constructor with public static factory `Create()` returning `ResultT<TValue>`
- Validation in Create() method
- Implicit operators for conversions (e.g., `string` conversion)
- `ToString()` override returning meaningful value

#### Result Pattern
- **Never throw exceptions for expected errors**
- Use `Result` for success/failure without value
- Use `ResultT<TValue>` for success with value / failure with errors
- Methods return `ResultT<TEntity>` for method chaining
- Error types: `Failure`, `NotFound`, `Validation`, `Conflict`, `AccessUnAuthorized`, `AccessForbidden`
- Error factory methods: `Error.Validation(code, description)`, `Error.NotFound(code, description)`, etc.
- Return `List<Error>` directly for failures (implicit operator handles conversion)

#### Features
- Partial classes split across multiple files
- Dependency injection via constructor
- Async methods for repository operations
- Request/Response DTOs as records
- Validate inputs before calling domain logic
- Check repository for existence conflicts
- Return `ResultT<T>` for all operations

#### Error Handling
- Define error constants in separate *Errors.cs files
- Pattern: `Namespace.ClassName.PropertyName` (e.g., `User.NameUnchanged`)
- Use `AddError()`, `AddErrors()`, `HasValidationErrors()`, `GetValidationErrors()`, `ClearValidationErrors()` helpers
- **Features** must call `entity.ClearValidationErrors()` before calling entity methods to ensure clean state
- Entities accumulate errors across method calls
- Return all accumulated errors when `HasValidationErrors()` is true

### Formatting & Structure
- File-scoped namespaces
- Collection expressions: `HashSet<Account> _accounts = []`
- Pattern matching: `if (existingUser is not null)`
- Null coalescing: `transactions ?? []`
- String interpolation: `$"User: {user.Id}"`
- Expression-bodied members where appropriate

### Repository Interfaces
- Located in `Infra/Repositories/`
- Async methods with `Async` suffix
- Return domain entities or `null`
- Common patterns: `GetByIdAsync`, `GetByEmailAsync`, `AddAsync`

### Testing
- Use xUnit framework
- Test classes in `Tests/` matching feature structure
- Use Moq for mocking repositories
- Test both success and failure paths
- Update tests when changing feature signatures
- Arrange-Act-Assert pattern preferred
- Test naming: `MethodName_ShouldReturnExpected_WhenCondition`

### File Organization
- Each entity has its own folder with related files
- Value objects typically in the same folder as their entity
- Errors defined in separate *Errors.cs files alongside the entity
- Features organized by entity with subfolders for operations (e.g., `Features/User/Create/Create.cs`)
- Repository interfaces in `Infra/Repositories/`

### Common Patterns

#### Domain Layer Creation
```csharp
public static ResultT<User> Create(string name, string email)
{
    var nameResult = UserFullName.Create(name);
    if (nameResult.IsFailure)
        return nameResult.Errors!;
    
    return new User(Guid.NewGuid(), nameResult.Value, emailResult.Value);
}
```

#### Feature Layer Operation
```csharp
public async Task<ResultT<CreateUserResult>> CreateUserAsync(CreateUserRequest request)
{
    var emailResult = Email.Create(request.Email);
    if (emailResult.IsFailure)
        return emailResult.Errors!;
    
    var existingUser = await _userRepository.GetByEmailAsync(emailResult.Value);
    if (existingUser is not null)
        return Error.Conflict(UserErrors.UserAlreadyExists, "User already exists");
    
    var userResult = User.Create(request.Name, request.Email);
    if (userResult.IsFailure)
        return userResult.Errors!;
    
    await _userRepository.AddAsync(userResult.Value);
    return new CreateUserResult(userResult.Value.Id);
}
```

#### Entity Update Method
```csharp
public ResultT<User> UpdateName(string newName)
{
    if (newName == _name.Full)
        AddError(Error.Validation(UserErrors.NameUnchanged, "Name must be different"));
    
    var nameResult = UserFullName.Create(newName);
    if (nameResult.IsFailure)
        AddErrors(nameResult.Errors!);
    
    if (HasValidationErrors())
        return GetValidationErrors();
    
    _name = nameResult.Value;
    UpdateTimestamp();
    return this;
}
```

#### Feature Layer with Error Clearing
```csharp
public async Task<ResultT<UpdateUserResult>> UpdateUserAsync(UpdateUserRequest request)
{
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user is null)
        return Error.NotFound(UserErrors.UserNotFound, "User not found");
    
    // Clear errors before entity operations
    user.ClearValidationErrors();
    
    var updateResult = user.UpdateName(request.Name);
    if (updateResult.IsFailure)
        return updateResult.Errors!;
    
    await _userRepository.UpdateAsync(user);
    return new UpdateUserResult(user.Id);
}
```

### Project Dependencies
- Domain: No external dependencies
- Features: Depends on Domain, Infra
- Infra: Depends on Domain (interfaces only)
- Tests: Depends on Domain, Features, Infra, xUnit, Moq
