using Discord.Commands;
using Discord;
using RedCunningFoxBot.Services;

namespace RedCunningFoxBot.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        AudioService audioService;
        
        public AudioModule(AudioService service)
        {
            audioService = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd(IVoiceChannel target = null)
        {
            target = target ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (target != null)
            {
                audioService.JoinChannel(Context.Guild, target, Context.Channel);
            }
            else
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument.");
                return;
            }
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd(IVoiceChannel channel = null)
        {
            audioService.LeaveChannel(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd(string path)
        {
            audioService.SendAsync(Context.Guild, path, Context.Channel, (Context.User as IGuildUser)?.VoiceChannel);
        }

        [Command("pause", RunMode = RunMode.Async)]
        public async Task PauseCmd(IVoiceChannel channel = null)
        {
            audioService.PauseAsync(Context.Guild);
        }

        [Command("unpause", RunMode = RunMode.Async)]
        public async Task UnpauseCmd(IVoiceChannel channel = null)
        {
            audioService.UnpauseAsync(Context.Guild);
        }

        [Command("seek", RunMode = RunMode.Async)]
        public async Task SeekCmd(IVoiceChannel channel = null)
        {
            audioService.SeekAsync(Context.Guild);
        }

        [Command("skip", RunMode = RunMode.Async)]
        public async Task SkipCmd(IVoiceChannel channel = null)
        {
            audioService.SkipAsync(Context.Guild);
        }
    }
}
