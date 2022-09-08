using Discord;
using Discord.Audio;
using System.Collections.Concurrent;
using Discord.WebSocket;
using System.Diagnostics;
using RedCunningFoxBot.Classes;

namespace RedCunningFoxBot.Services
{
    public class AudioService
    { 
        public AudioService()
        {

        }
        public async Task JoinChannel(IGuild guild, IVoiceChannel target, ISocketMessageChannel channel)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }
            IAudioClient audioClient = await target.ConnectAsync();
            player = new Player(audioClient);
            ConnectedChannels.TryAdd(guild.Id, player);
        }
        public async Task LeaveChannel(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out Player player))
            {
                await player.Stop();
            }
        }
        public async Task SendAsync(IGuild guild, string path, ISocketMessageChannel channel, IVoiceChannel VoiceChannel = null)
        {
            if (path == null)
            {
                return;
            }

            if (!ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                await JoinChannel(guild, VoiceChannel, channel);
                ConnectedChannels.TryGetValue(guild.Id, out player);
            }

            player.Add(path);

            player.Play();

        }
        public async Task PauseAsync(IGuild guild)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                player.Pause();
            }
        }
        public async Task UnpauseAsync(IGuild guild)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                player.Unpause();
            }
        }
        public async Task SeekAsync(IGuild guild)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                player.Seek();
            }
        }
        public async Task SkipAsync(IGuild guild)
        {
            if(ConnectedChannels.TryGetValue(guild.Id, out Player player))
            {
                player.Skip();
            }
        }

        #region private
        private readonly ConcurrentDictionary<ulong, Player> ConnectedChannels = new ConcurrentDictionary<ulong, Player>();
        #endregion
    }
}
