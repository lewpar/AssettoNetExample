using AssettoNet.Events;
using AssettoNet.Network;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AssettoNet.Example.Services;

internal class AssettoSpotService : BackgroundService
{
    private readonly AssettoClient _client;
    private readonly ILogger<AssettoSpotService> _logger;

    public AssettoSpotService(ILogger<AssettoSpotService> logger)
    {
        _client = new AssettoClient();

        _client.OnClientConnected += Client_OnClientConnected;
        _client.OnClientHandshake += Client_OnClientHandshake;
        _client.OnClientListening += Client_OnClientListening;
        _client.OnClientOperationEvent += Client_OnClientOperationEvent;

        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _client.ConnectAsync("localhost", 9996);
        await _client.ListenForEventsAsync(AssettoEventType.Spot);
    }

    private void Client_OnClientListening(object? sender, EventArgs e)
    {
        _logger.LogInformation("Listening for events..");
    }

    private void Client_OnClientOperationEvent(object? sender, AssettoOperationEventArgs e)
    {
        _logger.LogInformation("Received event: " + e.EventType);

        switch (e.EventType)
        {
            case AssettoEventType.Spot:
                _logger.LogInformation(e.Spot?.ToString());
                break;
        }
    }

    private void Client_OnClientHandshake(object? sender, AssettoHandshakeEventArgs e)
    {
        _logger.LogInformation("Passed handshake.");
    }

    private void Client_OnClientConnected(object? sender, EventArgs e)
    {
        _logger.LogInformation("Connected");
    }
}
