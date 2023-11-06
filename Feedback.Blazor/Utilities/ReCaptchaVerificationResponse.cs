using System.Text.Json.Serialization;

namespace Feedback.Blazor.Utilities;

public class ReCaptchaVerificationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset TimeStamp { get; set; }
    [JsonPropertyName("hostname")]
    public string? Hostname { get; set; }
    [JsonPropertyName("error-codes")]
    public string[]? ErrorCodes { get; set; }
}
