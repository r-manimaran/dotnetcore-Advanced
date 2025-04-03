
using StackExchange.Redis;
using System.Runtime.CompilerServices;

ConnectionMultiplexer _redis=null;

try
{
    _redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"); // Replace with your Redis connection string
    var db = _redis.GetDatabase();

    string key = "myKey";
    string value = Guid.NewGuid().ToString();
    TimeSpan ttl = TimeSpan.FromSeconds(10); // Set TTL to 10 seconds

    // Set the key with TTL
    await db.StringSetAsync(key, value, ttl);

    Console.WriteLine($"Key '{key}' set with value '{value}' and TTL of {ttl.TotalSeconds} seconds.");

    // Subscribe to expiration events
    var subscriber = _redis.GetSubscriber();
    await subscriber.SubscribeAsync("__keyevent@0__:expired", (channel, message) =>
    {
        if (message == key)
        {
            Console.WriteLine($"\nKey '{key}' expired! Triggering action...");
            // Add your trigger logic here
            TriggerAction(key);
        }
    });

    Console.WriteLine("Waiting for key expiration...");
    Console.ReadKey(); // Keep the application running
}
catch (RedisConnectionException ex)
{
    Console.WriteLine($"Redis connection error: {ex.Message}");
}
finally
{
    _redis?.Dispose();
}

static void TriggerAction(string expiredKey)
{
    // Implement your logic to be executed when the key expires
    Console.WriteLine($"\nTriggered action for expired key: {expiredKey}");
    // Example: Refreshing cache, logging, sending notifications, etc.
}