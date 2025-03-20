using AssettoNet.Events;
using AssettoNet.Network;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AssettoNet.Example.Services;

internal class AssettoUpdateService : BackgroundService
{
    private readonly AssettoClient _client;
    private readonly ILogger<AssettoUpdateService> _logger;

    public AssettoUpdateService(ILogger<AssettoUpdateService> logger)
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
        await _client.ListenForEventsAsync(AssettoEventType.Update);
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
            case AssettoEventType.Update:
                _logger.LogInformation(e.Update?.ToString());
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
