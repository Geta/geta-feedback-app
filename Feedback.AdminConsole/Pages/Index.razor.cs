using Feedback.Models;

namespace Feedback.AdminConsole.Pages;

public partial class Index
{
    private FeedbackOptions feedbackOptions = new();
    private FeedbackService? feedbackService;
    private IOrderedEnumerable<FeedbackModel.Feedback>? feedbacks;

    protected override void OnInitialized()
    {
        Configuration.GetSection(FeedbackOptions.Section).Bind(feedbackOptions);
        feedbackService = new(feedbackOptions);

        feedbacks = feedbackService.GetFeedbacksFromTable();
    }

    private void NavigateToFeedbackDetails(string Id)
    {
        Navigator.NavigateTo($"/feedback-details/{Id}");
    }
}
