using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MedizID.API.Common;
using MedizID.API.Models;

namespace MedizID.API.Services;

public interface IAuthService
{
    string GenerateJwtToken(ApplicationUser user);
    Task<ApplicationUser?> AuthenticateAsync(string email, string password);
}

public class JwtAuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtAuthService> _logger;

    public JwtAuthService(JwtSettings jwtSettings, ILogger<JwtAuthService> logger)
    {
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("role", user.Role.ToString()),
                new Claim("facilityId", user.FacilityId?.ToString() ?? "")
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Task<ApplicationUser?> AuthenticateAsync(string email, string password)
    {
        // This will be implemented when using UserManager in AuthService
        throw new NotImplementedException();
    }
}

public interface IAIService
{
    Task<(List<DiagnosisResult> diagnoses, string? reasoning)> GenerateDifferentialDiagnosisAsync(string symptoms, string medicalHistory);
}

public class DiagnosisResult
{
    public string Name { get; set; } = null!;
    public string Icd10 { get; set; } = null!;
    public int ConfidencePct { get; set; }
    public string Reason { get; set; } = null!;
    public List<string> SupportingFactors { get; set; } = new();
    public List<string> RefutingFactors { get; set; } = new();
}

public class AIService : IAIService
{
    private readonly string _apiKey;
    private readonly ILogger<AIService> _logger;

    public AIService(OpenAISettings settings, ILogger<AIService> logger)
    {
        _apiKey = settings.ApiKey;
        _logger = logger;
    }

    public async Task<(List<DiagnosisResult> diagnoses, string? reasoning)> GenerateDifferentialDiagnosisAsync(
        string symptoms, string medicalHistory)
    {
        try
        {
            // OpenAI API integration
            var prompt = $@"
You are a medical AI assistant helping with differential diagnosis.

Patient Symptoms: {symptoms}
Medical History: {medicalHistory}

Based on the symptoms and medical history provided, generate a list of the top 3-5 most likely diagnoses.

For each diagnosis, provide:
1. Diagnosis name
2. ICD-10 code (if applicable)
3. Confidence percentage (0-100)
4. Brief reason for consideration
5. Supporting factors
6. Refuting factors

Please format your response as a structured JSON array.
";

            _logger.LogInformation("Starting AI diagnosis generation with OpenAI API");

            // In production, make actual API call to OpenAI
            // For demonstration, return placeholder data
            await Task.CompletedTask;
            var diagnoses = new List<DiagnosisResult>
            {
                new DiagnosisResult
                {
                    Name = "Influenza",
                    Icd10 = "J11.1",
                    ConfidencePct = 75,
                    Reason = "Based on provided symptoms",
                    SupportingFactors = new List<string> { "Fever", "Cough" },
                    RefutingFactors = new List<string>()
                }
            };

            _logger.LogInformation("AI diagnosis generated successfully");

            return (diagnoses, prompt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating diagnosis with AI");
            throw;
        }
    }
}
