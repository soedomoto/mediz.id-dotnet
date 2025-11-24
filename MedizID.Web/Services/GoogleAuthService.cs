using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedizID.Web.Services;

/// <summary>
/// Service for handling Google OAuth authentication
/// </summary>
public interface IGoogleAuthService
{
    Task<bool> LoginWithGoogleAsync(string idToken);
    Task<GoogleLoginResponse?> ExchangeTokenAsync(string idToken);
}

public class GoogleAuthService : IGoogleAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<GoogleAuthService> _logger;
    private const string TokenKey = "jwt_token";
    private const string UserKey = "current_user";

    public GoogleAuthService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<GoogleAuthService> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _logger = logger;
    }

    /// <summary>
    /// Exchange Google ID token for application JWT token
    /// </summary>
    public async Task<GoogleLoginResponse?> ExchangeTokenAsync(string idToken)
    {
        try
        {
            var request = new { id_token = idToken };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/google", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GoogleLoginResponse>();
                _logger.LogInformation("Successfully exchanged Google token for JWT");
                return result;
            }

            _logger.LogWarning("Failed to exchange Google token. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exchanging Google token");
            return null;
        }
    }

    /// <summary>
    /// Login user with Google ID token
    /// </summary>
    public async Task<bool> LoginWithGoogleAsync(string idToken)
    {
        try
        {
            var googleResponse = await ExchangeTokenAsync(idToken);
            if (googleResponse == null)
            {
                _logger.LogWarning("Google token exchange returned null response");
                return false;
            }

            // Store JWT token
            await _localStorage.SetItemAsync(TokenKey, googleResponse.AccessToken);

            // Extract and store user info from token
            var user = ExtractUserFromToken(googleResponse.AccessToken);
            if (user != null)
            {
                await _localStorage.SetItemAsync(UserKey, user);
            }

            _logger.LogInformation("User {email} logged in successfully via Google", user?.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Google login");
            return false;
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
            _logger.LogError(ex, "Error extracting user from Google token");
            return null;
        }
    }
}

public class GoogleLoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
}
