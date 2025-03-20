using AssettoNet.Example.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AssettoNet.Example;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<AssettoSpotService>();
        builder.Services.AddHostedService<AssettoUpdateService>();

        var app = builder.Build();

        await app.RunAsync();
        await app.WaitForShutdownAsync();
    }
}
