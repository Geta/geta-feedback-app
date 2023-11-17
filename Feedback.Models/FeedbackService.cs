using Azure.Data.Tables;

namespace Feedback.Models;

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

    public IOrderedEnumerable<FeedbackModel.Feedback>? GetFeedbacksFromTable()
    {
        try
        {
            var table = TableClient();
            var feedbacks = table
                .Query<FeedbackModel.Feedback>(x => x.PartitionKey == "feedback-entry")
                .OrderByDescending(x => x.Timestamp);
            return feedbacks;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return null;
        }
    }

    public Azure.Pageable<FeedbackModel.Feedback>? GetFeedbackById(string Id)
    {
        try
        {
            var table = TableClient();
            var feedbacks = table
                .Query<FeedbackModel.Feedback>(x => x.RowKey == Id);
            return feedbacks;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return null;
        }
    }

    public void UpdateFeedback(string Id, FeedbackModel.Feedback newFeedback)
    {
        try
        {
            var table = TableClient();
            FeedbackModel.Feedback oldFeedback = table.GetEntity<FeedbackModel.Feedback>("feedback-entry", Id);
            oldFeedback.Status = newFeedback.Status;
            table.UpsertEntity(oldFeedback, TableUpdateMode.Merge);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
}
