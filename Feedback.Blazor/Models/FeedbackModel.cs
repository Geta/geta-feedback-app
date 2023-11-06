using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Feedback.Blazor.Models;

public class FeedbackModel
{
    public class Feedback : ITableEntity
    {
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }

        [Required]
        public string? Country { get; set; }
        [Required]
        public FeedbackStatuses? Status { get; set; }
        [Required]
        public string? CaptchaToken { get; set; }

        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
        public ETag ETag { get; set; } = default!;
    }

    public enum FeedbackStatuses
    {
        Open,
        Closed
    }
}
