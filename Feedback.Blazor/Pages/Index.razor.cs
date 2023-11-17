using Microsoft.AspNetCore.Components.Forms;
using Feedback.Blazor.Utilities;
using Feedback.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Feedback.Blazor.Pages;

public partial class Index
{
    private EditContext? _editContext;
    private FeedbackModel.Feedback? _feedbackModel = new();
    private FeedbackService? _feedbackService;
    private bool _loading;

    protected override void OnInitialized()
    {
        _feedbackModel ??= new();
        _editContext = new(_feedbackModel);
        _editContext.SetFieldCssClassProvider(new BootstrapValidationClassProvider());

        _feedbackService = new(Options);

        _feedbackModel.Country = Options.Country;
        _feedbackModel.Status = FeedbackModel.FeedbackStatuses.Open;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeAsync<int>(
                "googleReCaptcha",
                DotNetObjectReference.Create(this),
                "google-reCaptcha",
                ReCaptchaOptions.Value.SiteKey);
        }
    }

    private async Task AddRecord()
    {
        _loading = true;
        try
        {
            var verificationResponse = await VerifyReCaptcha();
            if (verificationResponse?.Success ?? false)
            {
                await _feedbackService!.AddFeedbackToTable(_feedbackModel!);
                Navigator.NavigateTo("thank-you");
                _loading = false;
            }
        }
        catch (Exception)
        {
            _loading = false;
            throw;
        }
    }

    [JSInvokable]
    public void CallbackOnSuccess(string response)
    {
        _feedbackModel!.CaptchaToken = response;
        _editContext!.Validate();
    }

    private async Task<ReCaptchaVerificationResponse?> VerifyReCaptcha()
    {
        var message = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "secret", ReCaptchaOptions.Value.Secret },
            { "response", _feedbackModel!.CaptchaToken! }
        });
        var client = ClientFactory.CreateClient();

        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", message);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var verificationResponse = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(responseContent);

        return verificationResponse;
    }
}