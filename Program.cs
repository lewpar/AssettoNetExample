using AssettoNet.Events;
using AssettoNet.Network;

namespace AssettoNet.Example;

internal class Program
{
    static async Task Main(string[] args)
    {
        var client = new AssettoClient();

        client.OnClientConnected += Client_OnClientConnected;
        client.OnClientHandshake += Client_OnClientHandshake;
        client.OnClientListening += Client_OnClientListening;
        client.OnClientOperationEvent += Client_OnClientOperationEvent;

        await client.ConnectAsync("localhost", 9996);
        await client.ListenForEventsAsync(AssettoEventType.Spot);
    }

    private static void Client_OnClientListening(object? sender, EventArgs e)
    {
        Console.WriteLine("Listening for events..");
    }

    private static void Client_OnClientOperationEvent(object? sender, AssettoOperationEventArgs e)
    {
        Console.WriteLine("Received event: " + e.EventType);

        switch (e.EventType)
        {
            case AssettoEventType.Spot:
                Console.WriteLine(e.Spot?.ToString());
                break;

            case AssettoEventType.Update:
                Console.WriteLine(e.Update?.ToString());
                break;
        }
    }

    private static void Client_OnClientHandshake(object? sender, AssettoHandshakeEventArgs e)
    {
        Console.WriteLine("Passed handshake.");
    }

    private static void Client_OnClientConnected(object? sender, EventArgs e)
    {
        Console.WriteLine("Connected");
    }
}
