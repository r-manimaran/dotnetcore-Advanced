using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShobHub.OrdersToDynamodb.Models;

public class OrderEvent
{
    public string MessageId { get; set; }
    public DateTime OccurredUtc { get; set; }
    public string Type { get; set; }
    public int Version { get; set; }
    public string CorrelationId { get; set; }
    public string PartitionKey { get; set; }
    public OrderPayload Payload { get; set; }
}

public class OrderPayload
{
    public string OrderId { get; set; }
    public string UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public List<string> ProductIds { get; set; }
    public Dictionary<string, int> Quantities { get; set; }
    public string Channel { get; set; }
    public string Region { get; set; }
    public DateTime CreatedUtc { get; set; }
}
