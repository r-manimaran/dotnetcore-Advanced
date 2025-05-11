using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Producer App");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();
/*
await channel.QueueDeclareAsync(
    queue: "messages-1",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

await channel.QueueDeclareAsync(
    queue: "messages-2",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);
*/
await channel.ExchangeDeclareAsync(
    exchange: "messages",
    durable: true,
    autoDelete: false,
    type: ExchangeType.Fanout);

await Task.Delay(10000);

for(int i=0;i<10; i++)
{
    var message = $"{DateTime.UtcNow} - {Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(message);

    /* // For Queue Publish
    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "messages-1",
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);

    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "messages-2",
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);
    */

    // Publish to Exchange
    await channel.BasicPublishAsync(
        exchange: "messages",
        routingKey: string.Empty,
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);

    Console.WriteLine($"Sent: {message}");

    await Task.Delay(2000);
}

Console.ReadLine();