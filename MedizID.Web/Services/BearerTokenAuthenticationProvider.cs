using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Blazored.LocalStorage;
using System.Text.Json;

namespace MedizID.Web.Services;

public class BearerTokenAuthenticationProvider : IAuthenticationProvider
{
    private readonly ILocalStorageService _localStorage;

    public BearerTokenAuthenticationProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userJson = await _localStorage.GetItemAsync<string>("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonSerializer.Deserialize<LoginResponse>(userJson);
                if (!string.IsNullOrEmpty(user?.Token))
                {
                    request.Headers.Add("Authorization", $"Bearer {user.Token}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to add bearer token: {ex.Message}");
        }
    }

    public Task<AllowedHostsValidator> GetAllowedHostsValidatorAsync(CancellationToken cancellationToken = default)
    {
        // Allow all hosts for development
        return Task.FromResult(new AllowedHostsValidator());
    }
}

public class LoginResponse
{
    public string? Token { get; set; }
    public string? User { get; set; }
}
