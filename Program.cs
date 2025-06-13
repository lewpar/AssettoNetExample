using AssettoNet.Network;

namespace AssettoNet.Example;

internal class Program
{
    static async Task Main(string[] args)
    {
        var client = new AssettoClient(host: "localhost", port: 9996);

        client.OnConnected += Client_OnConnected;
        client.OnDisconnected += Client_OnDisconnected;
        client.OnLapCompleted += Client_OnLapCompleted;
        client.OnPhysicsUpdate += Client_OnPhysicsUpdate;
        client.OnUnhandledException += Client_OnUnhandledException;

        Console.WriteLine("Waiting for UDP server..");
        while (!client.IsAssettoUdpServerListening())
        {
            await Task.Delay(100);
        }

        bool retryConnection = true;
        
        Console.WriteLine("Press Ctrl+C to disconnect.");
        Console.CancelKeyPress += async (sender, eventArgs) =>
        {
            if (client.IsConnected)
            {
                Console.WriteLine("Disconnecting..");
                retryConnection = false;
                await client.DisconnectAsync();
                Console.WriteLine("Press ENTER to exit.");
                Console.ReadLine();
            }
        };

        while (retryConnection)
        {
            if (!client.IsConnected &&
                client.IsAssettoUdpServerListening())
            {
                Console.WriteLine("Connecting to server..");
                await client.ConnectAsync();
            }
        }
    }

    private static void Client_OnDisconnected(object? sender, EventArgs e)
    {
        Console.WriteLine("Disconnected.");
    }

    private static void Client_OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;
        Console.WriteLine($"{ex.Message}: {ex.StackTrace}");
    }

    private static void Client_OnPhysicsUpdate(object? sender, Events.AssettoPhysicsUpdateEventArgs e)
    {
        //Console.Clear();
        //Console.WriteLine("[Physics Update]");
        //Console.WriteLine($"{e.Data.ToString()}");
    }

    private static void Client_OnLapCompleted(object? sender, Events.AssettoLapCompletedEventArgs e)
    {
        //Console.WriteLine();
        //Console.WriteLine($"[Completed Lap {e.Data.Lap}]");
        //Console.WriteLine($"{e.Data.ToString()}");
    }

    private static void Client_OnConnected(object? sender, Events.AssettoConnectedEventArgs e)
    {
        Console.WriteLine("Connected.");
        Console.WriteLine(e.Handshake.ToString());
    }
}
