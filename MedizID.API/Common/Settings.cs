namespace MedizID.API.Common;

public class JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationMinutes { get; set; } = 1440; // 24 hours
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
}

public class OpenAISettings
{
    public string ApiKey { get; set; } = null!;
    public string ApiBaseUrl { get; set; } = "https://api.openai.com/v1/";
    public string ModelName { get; set; } = "gpt-4.1-mini";
}

public class ApiSettings
{
    public string ApiVersion { get; set; } = "1.0.0";
    public string ProjectName { get; set; } = "mediz.id API";
}
