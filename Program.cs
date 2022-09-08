using System;
using Discord;
using Discord.WebSocket;
using Discord.Addons.Hosting;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RedCunningFoxBot.Services;

namespace RedCunningFox
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureDiscordHost((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 200
                    };
                    config.Token = Environment.GetEnvironmentVariable("RED_FOX_DISCORD_TOKEN", EnvironmentVariableTarget.User);
                })
                .UseCommandService((context, config) =>
                {
                    config.DefaultRunMode = RunMode.Async;
                    config.CaseSensitiveCommands = false;
                })
                .UseInteractionService((context, config) =>
                {
                    config.LogLevel = LogSeverity.Info;
                    config.UseCompiledLambda = true;
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<CommandHandler>().AddHttpClient();
                    services.AddHostedService<BotStatusService>();
                    services.AddSingleton(new AudioService());
                }).Build();
            

            await host.RunAsync();
        }

    }
}
