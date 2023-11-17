using Microsoft.AspNetCore.Components;
using Feedback.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Feedback.AdminConsole.Pages;

public partial class FeedbackDetails
{
    [Parameter]
    public string FeedbackID { get; set; }

    private EditContext? editContext;
    private FeedbackModel.Feedback? feedbackModel;

    private FeedbackOptions feedbackOptions = new();
    private FeedbackService? feedbackService;
    private Azure.Pageable<FeedbackModel.Feedback>? feedbacks;

    protected override void OnInitialized()
    {
        feedbackModel = new();
        editContext = new(feedbackModel);

        Configuration.GetSection(FeedbackOptions.Section).Bind(feedbackOptions);
        feedbackService = new(feedbackOptions);

        feedbacks = feedbackService.GetFeedbackById(FeedbackID);
    }

    private void UpdateRecord()
    {
        feedbackService.UpdateFeedback(FeedbackID, feedbackModel);
    }
}
