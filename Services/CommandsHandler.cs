using Discord;
using Discord.WebSocket;
using Discord.Addons.Hosting;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace RedCunningFoxBot.Services
{
    internal class CommandHandler : DiscordClientService
    {
        private readonly IServiceProvider provider;
        private readonly CommandService service;
        private readonly IConfiguration config;

        public CommandHandler(DiscordSocketClient client, ILogger<CommandHandler> logger, IServiceProvider provider, CommandService commandService, IConfiguration config) : base(client, logger)
        {
            this.provider = provider;
            this.service = commandService;
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.MessageReceived += onMessageReceived;
            await service.AddModulesAsync(Assembly.GetEntryAssembly(), this.provider);
        }

        private async Task onMessageReceived(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message))
            {
                return;
            }
            if (message.Source != MessageSource.User)
            {
                return;
            }
            var argPos = 0;
            if (!message.HasStringPrefix(System.Configuration.ConfigurationManager.AppSettings["Prefix"], ref argPos) && !message.HasMentionPrefix(this.Client.CurrentUser, ref argPos))
            {
                return;
            }
            var context = new SocketCommandContext(this.Client, message);
            await this.service.ExecuteAsync(context, argPos, this.provider);
        }
    }
}
