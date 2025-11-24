using System.Net.Http.Json;

namespace MedizID.Web.Services;

public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
    Task DeleteAsync(string endpoint);
    Task<List<T>> GetListAsync<T>(string endpoint);
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authService;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, IAuthenticationService authService, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _authService = authService;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during GET request to {endpoint}", endpoint);
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();

            _logger.LogWarning("POST request failed: {endpoint} - {statusCode}", endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during POST request to {endpoint}", endpoint);
            return default;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();

            _logger.LogWarning("PUT request failed: {endpoint} - {statusCode}", endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PUT request to {endpoint}", endpoint);
            return default;
        }
    }

    public async Task DeleteAsync(string endpoint)
    {
        try
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("DELETE request failed: {endpoint} - {statusCode}", endpoint, response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during DELETE request to {endpoint}", endpoint);
        }
    }

    public async Task<List<T>> GetListAsync<T>(string endpoint)
    {
        try
        {
            var result = await GetAsync<List<T>>(endpoint);
            return result ?? new List<T>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving list from {endpoint}", endpoint);
            return new List<T>();
        }
    }

    private async Task AddAuthorizationHeader()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
