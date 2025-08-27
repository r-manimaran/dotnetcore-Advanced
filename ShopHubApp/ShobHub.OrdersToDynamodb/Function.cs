using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using ShobHub.OrdersToDynamodb.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ShobHub.OrdersToDynamodb;

public class Function
{
    private static readonly AmazonDynamoDBClient _ddb = new();

    private static readonly string _table = Environment.GetEnvironmentVariable("DDB_TABLE")!;

    public async Task FunctionHandler(KinesisEvent kinesisEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Beginning to process {kinesisEvent.Records.Count} records...");

        foreach (var record in kinesisEvent.Records)
        {
            context.Logger.LogInformation($"Event ID: {record.EventId}");

            context.Logger.LogInformation($"Event Name: {record.EventName}");
            try
            {
                string recordData = GetRecordContents(record.Kinesis);

                context.Logger.LogInformation($"Processed record:{recordData}");

                var orderEvent = JsonSerializer.Deserialize<OrderEvent>(recordData);

                await SaveOrderToDynamoDB(orderEvent, context);

                context.Logger.LogInformation("Inserted to DynamoDb Successfully");
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error Processing record {record.EventId} : {ex.Message}");
            }            
         
        }

        context.Logger.LogInformation("Stream processing complete.");
    }

    private string GetRecordContents(KinesisEvent.Record streamRecord)
    {
        using (var reader = new StreamReader(streamRecord.Data, Encoding.ASCII))
        {
            return reader.ReadToEnd();
        }
    }

    private async Task SaveOrderToDynamoDB(OrderEvent orderEvent, ILambdaContext context)
    {
        try
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["OrderId"] = new AttributeValue { S = orderEvent.Payload.OrderId },
                ["UserId"] = new AttributeValue { S = orderEvent.Payload.UserId },
                ["TotalAmount"] = new AttributeValue { N = orderEvent.Payload.TotalAmount.ToString() },
                ["Currency"] = new AttributeValue { S = orderEvent.Payload.Currency },
                ["Channel"] = new AttributeValue { S = orderEvent.Payload.Channel },
                ["Region"] = new AttributeValue { S = orderEvent.Payload.Region },
                ["CreatedUtc"] = new AttributeValue { S = orderEvent.Payload.CreatedUtc.ToString("O") },
                ["ProcessedUtc"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") }
            };

            var request = new PutItemRequest
            {
                TableName = _table,
                Item = item
            };

            await _ddb.PutItemAsync(request);
            context.Logger.LogInformation($"Successfully saved order {orderEvent.Payload.OrderId} to DynamoDB");

        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error saving to DynamoDb:{ex.Message}");
            throw;
        }
    }
}