using Microsoft.AspNetCore.Components.WebAssembly.Http;
using SharedLib;
using System.Net.ServerSentEvents;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace BlazorAppUI.Services;

public class HumidityService : IHumidityService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Channel<int> _channel;
    public HumidityService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _channel = Channel.CreateUnbounded<int>();
    }

    public ChannelReader<int> GetHumidityStream => _channel.Reader;

    public async Task StartAsync()
    {
        var client = _httpClientFactory.CreateClient("WebApiClient");

        var request = new HttpRequestMessage(HttpMethod.Get, "humidity/stream");

        request.SetBrowserResponseStreamingEnabled(true);

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        using var stream = await response.Content.ReadAsStreamAsync();

        var parser = SseParser.Create(stream, (type, data) =>
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<Humidity>(json);
        });

        await foreach (var sseEvent in parser.EnumerateAsync())
        {
            var value = sseEvent.Data?.Percentage ?? 0;
            await _channel.Writer.WriteAsync(value);
        }
        _channel.Writer.Complete();
    }
}
