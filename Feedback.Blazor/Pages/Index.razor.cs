using Microsoft.AspNetCore.Components.Forms;
using Feedback.Blazor.Utilities;
using Feedback.Blazor.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Feedback.Blazor.Pages;

public partial class Index
{
    private EditContext? editContext;
    private FeedbackModel.Feedback? FeedbackModel = new();
    private FeedbackOptions feedbackOptions = new();
    private FeedbackService? feedbackService;

    private GoogleReCaptchaOptions googleReCaptchaOptions = new();

    private bool Loading = false;

    protected override void OnInitialized()
    {
        FeedbackModel ??= new();
        editContext = new(FeedbackModel);
        editContext.SetFieldCssClassProvider(new BootstrapValidationClassProvider());

        Configuration.GetSection(FeedbackOptions.Section).Bind(feedbackOptions);
        feedbackService = new(feedbackOptions);
        
        FeedbackModel.Country = feedbackOptions.Country;
        FeedbackModel.Status = Models.FeedbackModel.FeedbackStatuses.Open;

        Configuration.GetSection(GoogleReCaptchaOptions.Section).Bind(googleReCaptchaOptions);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeAsync<int>("googleReCaptcha", DotNetObjectReference.Create(this), "google-reCaptcha", googleReCaptchaOptions.SiteKey);
        }
    }

    private async Task AddRecord()
    {
        Loading = true;
        try
        {
            var verificationResponse = await VerifyReCaptcha();
            if (verificationResponse.Success)
            {
                await feedbackService!.AddFeedbackToTable(FeedbackModel!);
                Navigator.NavigateTo("thank-you");
                Loading = false;
            }
        }
        catch (Exception)
        {
            Loading = false;
            throw;
        }
    }

    [JSInvokable]
    public void CallbackOnSuccess(string response)
    {
        FeedbackModel!.CaptchaToken = response;
        editContext!.Validate();
    }

    private async Task<ReCaptchaVerificationResponse?> VerifyReCaptcha()
    {
        var message = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "secret", googleReCaptchaOptions.Secret },
            { "response", FeedbackModel!.CaptchaToken! }
        });
        var client = ClientFactory.CreateClient();
        
        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", message);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var verificationResponse = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(responseContent);
        return verificationResponse;
    }
}
