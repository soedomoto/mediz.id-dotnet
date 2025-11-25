# MedizID.Web - Blazor WebAssembly Frontend

This is a fresh Blazor WebAssembly application for the MedizID clinic management system, featuring:

## ✅ Completed Features

### Project Setup
- ✅ Blazor WebAssembly 8.0 application
- ✅ Kiota-generated API client for type-safe API communication
- ✅ Blazored.LocalStorage for persistent client-side storage
- ✅ All dependencies installed and configured

### API Integration
- ✅ **Kiota Code Generation**: Automatically generated C# API client from OpenAPI schema
  - Generated from: `http://localhost:5053/swagger/v1/swagger.json`
  - Location: `/Services/Generated/`
  - Includes all models and request/response types

### Authentication Pages
1. **Login Page** (`/Pages/Login.razor`)
   - Email and password form
   - Integration with API Auth/Login endpoint
   - Token storage in local storage
   - Error handling and loading states
   - Redirect to home on successful login

2. **Register Page** (`/Pages/Register.razor`)
   - First name, last name, email, password fields
   - Password confirmation validation
   - Integration with API Auth/Register endpoint
   - Comprehensive form validation
   - Success message and redirect to login

3. **Home Page** (`/Pages/Home.razor`)
   - Welcome screen for unauthenticated users
   - Login/Register buttons
   - Authenticated view with logout functionality
   - User info display after login

## 📁 Project Structure

```
MedizID.Web/
├── Pages/
│   ├── Home.razor              # Home page with auth/unauth views
│   ├── Login.razor             # Login page with form
│   ├── Register.razor          # Registration page with form
│   └── Counter.razor           # (default empty template)
├── Layout/
│   ├── MainLayout.razor        # Main layout component
│   └── NavMenu.razor           # Navigation menu
├── Services/
│   └── Generated/              # Kiota-generated API client
│       ├── MedizIdApiClient.cs # Main API client class
│       ├── Api/                # API endpoint builders
│       │   ├── V1/
│       │   │   ├── Auth/       # Authentication endpoints
│       │   │   ├── Patients/
│       │   │   ├── Appointments/
│       │   │   └── ...         # Other endpoints
│       │   └── ApiRequestBuilder.cs
│       └── Models/             # Generated DTOs
│           ├── LoginRequest.cs
│           ├── LoginResponse.cs
│           ├── RegisterRequest.cs
│           ├── RegisterResponse.cs
│           └── ...             # All other models
├── App.razor                   # Router and app shell
├── Program.cs                  # DI configuration and startup
├── _Imports.razor              # Global using statements
├── MedizID.Web.csproj          # Project file
└── wwwroot/
    └── index.html              # Entry point

```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- API running on `http://localhost:5053`

### Build Instructions

```bash
# Build the project
cd MedizID.Web
dotnet build

# Run in development mode
dotnet run

# The app will be available at https://localhost:7123 (or similar)
```

### Release Build
```bash
dotnet build --configuration Release
# Output: bin/Release/net8.0/wwwroot/
```

## 🔧 Configuration

### Program.cs - Key Service Registrations

1. **HttpClient**: Default HTTP client for general use
2. **Blazored.LocalStorage**: For client-side storage of tokens
3. **Kiota Authentication Provider**: Anonymous (can be upgraded to JWT)
4. **MedizIdApiClient**: Main API client configured with:
   - Base address: `http://localhost:5053`
   - JSON serialization/deserialization
   - Anonymous authentication (ready for JWT upgrade)

### API Client Usage Example

```csharp
// In Login.razor
var response = await ApiClient.Api.V1.Auth.Login.PostAsync(loginRequest);

// In Register.razor
var response = await ApiClient.Api.V1.Auth.Register.PostAsync(registerRequest);
```

## 📦 Dependencies

### NuGet Packages Added
- `Microsoft.Kiota.Bundle` (1.21.0) - Includes all Kiota dependencies:
  - Microsoft.Kiota.Abstractions
  - Microsoft.Kiota.Http.HttpClientLibrary
  - Microsoft.Kiota.Serialization.Json
  - Microsoft.Kiota.Serialization.Form
  - Microsoft.Kiota.Serialization.Text
  - Microsoft.Kiota.Serialization.Multipart
- `Blazored.LocalStorage` (4.5.0) - Client-side storage

## ✨ Build Status

✅ **Release Build**: Successful (0 Warnings, 0 Errors)
✅ **Debug Build**: Successful (0 Warnings, 0 Errors)

## 🔐 Security Notes

### Current State
- Authentication provider is set to `AnonymousAuthenticationProvider`
- Tokens are stored in browser local storage

### Future Enhancements
- Implement JWT token-based authentication
- Add HttpMessageHandler for automatic token injection
- Implement token refresh mechanism
- Add authentication state provider for role-based access
- Protect pages with authorization attributes

## 🔄 Regenerating API Client

If the backend API changes:

```bash
# Regenerate using Kiota
kiota generate \
  --openapi "http://localhost:5053/swagger/v1/swagger.json" \
  --language csharp \
  --class-name "MedizIdApiClient" \
  --output "Services/Generated" \
  --namespace-name "MedizID.Web.Services.Generated" \
  --clean-output
```

## 📋 API Endpoints Supported

The generated client supports all endpoints from the API including:
- ✅ `/api/v1/Auth/login` - POST
- ✅ `/api/v1/Auth/register` - POST
- ✅ All other endpoints (Patients, Appointments, Medical Records, etc.)

## 🧪 Testing

Run the application:
```bash
dotnet run
```

Navigate to:
- `http://localhost:5173/` (or configured port) - Home page
- `/login` - Login page
- `/register` - Registration page

## 📝 Notes

- All components use generated Kiota types for strong typing
- Error handling is implemented on all forms
- Loading states provide user feedback
- Local storage persists authentication tokens
- Responsive design with gradient backgrounds

## 🎯 Next Steps

1. **Test Authentication Flow**
   - Test login with valid credentials
   - Test register with new account
   - Verify token storage and retrieval

2. **Implement JWT Token Handling**
   - Add Bearer token to API requests
   - Implement token refresh logic
   - Add logout with token cleanup

3. **Add Protected Routes**
   - Implement AuthorizeView components
   - Add authentication state provider
   - Protect pages with [Authorize] attribute

4. **Enhance UI/UX**
   - Add more pages and components
   - Implement dashboard
   - Add patient management UI
   - Create appointment scheduling

5. **Error Handling**
   - Implement global error boundary
   - Add detailed error messages
   - Create error notification service

---

**Build Date**: November 25, 2025
**Framework**: .NET 8.0
**Status**: ✅ Ready for development
