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
}

public class ApiSettings
{
    public string ApiVersion { get; set; } = "1.0.0";
    public string ProjectName { get; set; } = "mediz.id API";
}
