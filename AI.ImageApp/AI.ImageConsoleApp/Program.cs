using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;


var builder = Host.CreateApplicationBuilder();
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string openApiKey = config["OPENAIKEY"];
string openApiModelName = config["ModelName"];

// To Register Ollama 
//builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"),"llava:7b"));

// To Register OpenAI
builder.Services.AddChatClient(new OpenAIChatClient(new OpenAI.OpenAIClient(openApiKey),openApiModelName));


var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();
// Example 1
/*var chatMessage = new ChatMessage(ChatRole.User, "what's in this image?");
chatMessage.Contents.Add(new DataContent(File.ReadAllBytes("Images/Image1.png"),"image/png"));

var response = await chatClient.GetResponseAsync(chatMessage);
Console.WriteLine(response);*/

// Example 2:
/*
var chatMessage2 = new ChatMessage(ChatRole.User, "Can you count the vehicles in the image by category?");
chatMessage2.Contents.Add(new DataContent(File.ReadAllBytes("Images/Image2.jpg"), "image/jpg"));

var response2 = await chatClient.GetResponseAsync(chatMessage2);
Console.WriteLine(response2);
Console.ReadLine();*/


foreach(var file in Directory.GetFiles("Images","*.*"))
{
    var name = Path.GetFileNameWithoutExtension(file);
    var message = new ChatMessage(ChatRole.User,
        $"""
        Extract information from this image from camera {name}.
        Pay extra attention to columns of cars and try to accurately count the unumber of cars.
        If there is any Petestrian spot them and count them as well separately.
        """);
    message.Contents.Add(new DataContent(File.ReadAllBytes(file), "image/jpg"));
    var trafficCamResult = await chatClient.GetResponseAsync<TrafficCamResult>(message);
    Console.WriteLine(trafficCamResult.Result with { CameraName = name });
}

Console.ReadLine();
public record TrafficCamResult
{
    public string CameraName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrafficStatus Status { get; set; }
    public int NumberOfCars { get; set; }
    public int NumberOfTrucks { get; set; }
    public int NumberOfPedestrians { get; set; }

    public enum TrafficStatus
    {
        Empty,
        Normal,
        Heavy,
        Blocked
    }
}
     
