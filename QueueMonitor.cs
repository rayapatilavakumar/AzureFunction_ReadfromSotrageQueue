using Azure;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunction_BlobReader
{
    public class QueueMonitor
    {
        private readonly ILogger<QueueMonitor> _logger;

        public QueueMonitor(ILogger<QueueMonitor> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueMonitor))]
        [TableOutput("ImageDetails", Connection = "rlk-azurefunction")]
        public ReviewImageInfo Run([QueueTrigger("rlk-storagequeue")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            return new ReviewImageInfo
            {
                PartitionKey= Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                UploadedDate = DateTime.Now,
                ImageName = message.MessageText
            };
        }
    }

    public class ReviewImageInfo : Azure.Data.Tables.ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ImageName { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Text { get; set; }
    }
}
