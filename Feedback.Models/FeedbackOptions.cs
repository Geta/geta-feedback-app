namespace Feedback.Models;

public class FeedbackOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string Country {  get; set; } = string.Empty;

    public static string Section = "Feedback";
}
