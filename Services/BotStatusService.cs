using Discord.WebSocket;
using Discord.Addons.Hosting;
using Microsoft.Extensions.Logging;
using Discord.Addons.Hosting.Util;

namespace RedCunningFoxBot.Services
{
    internal class BotStatusService : DiscordClientService
    {
        public BotStatusService(DiscordSocketClient client, ILogger<DiscordClientService> logger) : base(client, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);
            await Client.SetGameAsync($"{System.Configuration.ConfigurationManager.AppSettings["Prefix"]}help");
        }
    }
}
