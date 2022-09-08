using Discord.Commands;
using RedCunningFoxBot.Common;

namespace RedCunningFoxBot.Modules
{
    internal class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {
            string firstColumn = "hello\nflipcoin";
            string secondColumn = "ping\nrolldice";
            string thirdColumn = "random";
            var embed = new RedFoxEmbedBuilder()
                .WithTitle("Информация")
                .AddField("Стандартные", firstColumn, true)
                .AddField("*", secondColumn, true)
                .AddField("*", thirdColumn, true)
                .WithFooter("Больше информации по командам f!help [command]")
                .Build();
            await ReplyAsync(embed: embed);
        }
    }
}
