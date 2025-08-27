using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Contracts.Events;

public record EventEnvelope<T>(string MessageId, DateTime OccurredUtc, string Type, int Version, 
                               string CorrelationId, string PartitionKey, T Payload);


public record OrderCreatedV1(string OrderId, string UserId, decimal TotalAmount, string Currency, 
                            string[] ProductIds, IDictionary<string,int> Quantities,
                            string Channel,
                            string Region, DateTime createdUtc);

public record PaymentResultV1(string OrderId, string UserId, bool Succeeded, string Provider, string AuthorizationCode,
                              string FailureReason, DateTime OccuredUtc);

