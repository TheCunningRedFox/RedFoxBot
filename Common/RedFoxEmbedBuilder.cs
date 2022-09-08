using Discord;

namespace RedCunningFoxBot.Common
{
    internal class RedFoxEmbedBuilder : EmbedBuilder
    {
        public RedFoxEmbedBuilder ()
        {
            this.WithColor(new Color(240, 150, 14));
        }
    }
}
