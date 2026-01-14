using System.Threading.Channels;

namespace BlazorAppUI.Services;

public interface IHumidityService
{
    ChannelReader<int> GetHumidityStream { get; }
    Task StartAsync();
}