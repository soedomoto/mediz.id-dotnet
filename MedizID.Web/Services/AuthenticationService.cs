using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace MedizID.Web.Services;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<bool> RegisterAsync(string email, string firstName, string lastName, string password, string phoneNumber);
    Task<AuthUser?> GetCurrentUserAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<AuthenticationService> _logger;
    private const string TokenKey = "jwt_token";
    private const string UserKey = "current_user";
    private AuthUser? _currentUser;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<AuthenticationService> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var loginRequest = new { email, password };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await _localStorage.SetItemAsync(TokenKey, result.AccessToken);
                    
                    // Extract user info from token
                    var user = ExtractUserFromToken(result.AccessToken);
                    if (user != null)
                    {
                        await _localStorage.SetItemAsync(UserKey, user);
                        _currentUser = user;
                    }

                    _logger.LogInformation("User {email} logged in successfully", email);
                    return true;
                }
            }

            _logger.LogWarning("Login failed for user {email}", email);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            await _localStorage.RemoveItemAsync(UserKey);
            _currentUser = null;
            _logger.LogInformation("User logged out successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
        }
    }

    public async Task<bool> RegisterAsync(string email, string firstName, string lastName, string password, string phoneNumber)
    {
        try
        {
            var registerRequest = new { email, firstName, lastName, password, phoneNumber };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User {email} registered successfully", email);
                return true;
            }

            _logger.LogWarning("Registration failed for user {email}", email);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return false;
        }
    }

    public async Task<AuthUser?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        try
        {
            _currentUser = await _localStorage.GetItemAsync<AuthUser>(UserKey);
            return _currentUser;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            return jwtToken?.ValidTo > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }
        catch
        {
            return null;
        }
    }

    private AuthUser? ExtractUserFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var user = new AuthUser
            {
                Id = Guid.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? Guid.Empty.ToString()),
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty,
                FirstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
                LastName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty,
                FacilityId = Guid.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "facility_id")?.Value ?? Guid.Empty.ToString()),
            };

            var rolesClaim = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            user.Roles = rolesClaim.Select(c => c.Value).ToList();

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting user from token");
            return null;
        }
    }
}

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}

public class AuthUser
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public Guid FacilityId { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
