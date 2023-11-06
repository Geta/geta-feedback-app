using Feedback.Blazor.Models;
using Azure.Data.Tables;

namespace Feedback.Blazor.Utilities;

public class FeedbackService
{
    private string connectionString;
    private string tableName;
    public FeedbackService(FeedbackOptions feedbackOptions)
    {
        connectionString = feedbackOptions.ConnectionString;
        tableName = feedbackOptions.TableName;
    }
    private TableClient TableClient()
    {

        var tableClient = new TableClient(connectionString, tableName);
        return tableClient;
    }

    public async Task AddFeedbackToTable(FeedbackModel.Feedback feedback)
    {
        try
        {
            feedback.PartitionKey = "feedback-entry";
            feedback.RowKey = Guid.NewGuid().ToString();
            var table = TableClient();
            var response = await table.AddEntityAsync(feedback);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
}
