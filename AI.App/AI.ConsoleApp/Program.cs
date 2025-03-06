using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"),
                                                        "llama3"));
var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

var chatCompletion = await chatClient.CompleteAsync("What is fluentd?. Explain in 100 words");
// Console.WriteLine(chatCompletion.Message.Text);
// Console.Read();

var chatHistory = new List<ChatMessage>();
while(true)
{
    Console.WriteLine("Your Prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    // Stream the AI response and add to chat history
    var chatResponse = "";
    await foreach(var item in chatClient.CompleteStreamingAsync(chatHistory))
    {
        Console.Write(item.Text);
        chatResponse += item.Text;
    }

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatResponse));
    Console.WriteLine();
}


var posts = Directory.GetFiles("posts").Take(5).ToArray();
foreach(var post in posts)
{
    string prompt =
        $$"""
        
        # Desired Response
        Only provide a RFC8259 compliant JSON response following this format deviation.
        {
         "title":"Title pulled from the post",
         "summary":"summarize the article in no more than 100 words"
        }

        # Article content        
        {{File.ReadAllText(post)}}
        """;
    var chatCompletion2 = await chatClient.CompleteAsync(prompt);

    Console.WriteLine(chatCompletion2.Message.Text);
    Console.WriteLine(Environment.NewLine);
}