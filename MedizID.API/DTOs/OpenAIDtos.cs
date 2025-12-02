using System.Text.Json.Serialization;

namespace MedizID.API.DTOs;

public class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public List<OpenAIChoice> Choices { get; set; } = new();
}

public class OpenAIChoice
{
    [JsonPropertyName("message")]
    public OpenAIMessage Message { get; set; } = null!;
}

public class OpenAIMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}