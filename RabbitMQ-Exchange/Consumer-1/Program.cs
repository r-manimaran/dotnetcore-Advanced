using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer App 1:");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "messages",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.WriteLine("Waiting for the messages...");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Received: {message}");

    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync("messages",autoAck: false, consumer);

Console.ReadLine();
Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
{
    throw new NotImplementedException();
}