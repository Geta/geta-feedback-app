namespace Feedback.Blazor.Utilities;

public class GoogleReCaptchaOptions
{
    public const string Section = "GoogleReCaptcha";

    public string SiteKey { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
