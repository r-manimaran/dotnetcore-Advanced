using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Producer App");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "messages",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

await Task.Delay(1000);

for(int i=0;i<10; i++)
{
    var message = $"{DateTime.UtcNow} - {Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "messages",
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);

    Console.WriteLine($"Sent: {message}");

    await Task.Delay(2000);
}

Console.ReadLine();