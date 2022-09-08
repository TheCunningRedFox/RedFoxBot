using Discord.Commands;
using RedCunningFoxBot.Common;
using RedCunningFoxBot.Models;

namespace RedCunningFoxBot.Modules
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public GeneralModule(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [Command("flipcoin")]
        public async Task FlipCoinAsync()
        {
            Random rand = new Random();
            int coin = rand.Next(2);
            string result = coin == 0 ? "Орел" : "Решка";
            await ReplyAsync(result);
        }

        [Command("hello")]
        public async Task HelloAsync()
        {
            await ReplyAsync($"Привет {Context.User.Mention}");
        }

        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong!");
        }

        [Command("rolldice")]
        public async Task RollDiceAsync()
        {
            Random rand = new Random();
            int dice = rand.Next(6) + 1;
            string result = dice.ToString();
            await ReplyAsync(result);
        }

        [Command("random")]
        public async Task RandomAsync()
        {
            Random rand = new Random();
            int random = rand.Next();
            string result = random.ToString();
            await ReplyAsync(result);
        }

        [Command("quote")]
        public async Task QuoteAsync()
        {
            var httpClient = this.httpClientFactory.CreateClient();
            var responce = await httpClient.GetStringAsync("http://api.forismatic.com/api/1.0/?method=getQuote&format=json&lang=ru");
            var quote = Quote.FromJson(responce);
            string author = quote.QuoteAuthor == String.Empty ? "Автор неизвестен" : quote.QuoteAuthor;
            var embed = new RedFoxEmbedBuilder().AddField(quote.QuoteText, $"*{author}*", true).WithFooter("http://forismatic.com").Build();
            await ReplyAsync(embed: embed);
        }
    }
}
