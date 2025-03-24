using AssettoNet.Network;

namespace AssettoNet.Example;

internal class Program
{
    static async Task Main(string[] args)
    {
        var client = new AssettoClient();

        client.OnConnected += Client_OnConnected;
        client.OnLapCompleted += Client_OnLapCompleted;
        client.OnPhysicsUpdate += Client_OnPhysicsUpdate;
        client.OnUnhandledException += Client_OnUnhandledException;

        Console.WriteLine("Connecting to server..");

        await client.ConnectAsync(host: "localhost", port: 9996);
        await client.ListenForEventsAsync();

        await Task.Delay(TimeSpan.FromSeconds(120));

        await client.DisconnectAsync();
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
        Console.WriteLine();
        Console.WriteLine($"[Completed Lap {e.Data.Lap}]");
        Console.WriteLine($"{e.Data.ToString()}");
    }

    private static void Client_OnConnected(object? sender, Events.AssettoConnectedEventArgs e)
    {
        Console.WriteLine("Connected.");
        Console.WriteLine(e.Handshake.ToString());
    }
}
