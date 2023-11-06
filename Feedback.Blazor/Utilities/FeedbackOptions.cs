namespace Feedback.Blazor.Utilities;

public class FeedbackOptions
{
    public const string Section = "Feedback";

    public string ConnectionString { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string Country {  get; set; } = string.Empty;
}
