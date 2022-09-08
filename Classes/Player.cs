using Discord.Audio;
using System.Diagnostics;

namespace RedCunningFoxBot.Classes
{
    internal class Player
    {
        private readonly IAudioClient audioClient;
        public List<string> playlist { get; }
        private EStatus state { get; set; }

        public Player(IAudioClient audioClient)
        {
            this.audioClient = audioClient;
            this.playlist = new List<string>();
            this.state = EStatus.empty;
        }
        public void Add(string path)
        {
            playlist.Add(path);
            if(state == EStatus.empty)
            {
                state = EStatus.pause;
            }
        }
        public void Remove(string path)
        {
            playlist.Remove(path);
            if (playlist.Count == 0)
            {
                state = EStatus.empty;
            }
        }
        public void Clear()
        {
            playlist.Clear();
            state = EStatus.empty;
        }
        public void Pause()
        {
            if(state == EStatus.pause)
            {
                return;
            }
            state = EStatus.pause;
            new Thread(() => _tcs.TrySetResult(true)).Start();
        }
        public void Unpause()
        {
            if (state != EStatus.pause)
            {
                return;
            }
            state = EStatus.playing;
            new Thread(() => _tcs.TrySetResult(false)).Start();
        }
        public void Skip()
        {
            if (state == EStatus.empty)
            {
                return;
            }
            skip = true;
        }
        public void Seek()
        {

        }
        async public Task Stop()
        {
            await audioClient.StopAsync();
        }
        async public Task Play()
        {
            if (state == EStatus.playing)
            {
                return;
            }
            if (playlist.Count == 0)
            {
                return;
            }
            state = EStatus.playing;
            await Playing();
        }

        #region private
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
        private CancellationTokenSource _disposeToken = new CancellationTokenSource();
        private const int bufferSize = 1024;
        private const int bytesSent = 0;
        private byte[] buffer = new byte[bufferSize];
        private bool exit = false;
        private bool skip = false;
        private int seek = 0;
        async private Task Playing()
        {
            using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed))
            {
                try
                {
                    while (playlist.Count > 0)
                    {
                        string track = GetNextTrack();
                        exit = false;
                        skip = false;
                        using (var ffmpeg = CreateStream(track))
                        using (var output = ffmpeg.StandardOutput.BaseStream)
                        {
                            while (!exit || !skip)
                            {
                                int read = await output.ReadAsync(buffer, 0, bufferSize);
                                if (read == 0)
                                {
                                    exit = true;
                                    break;
                                }
                                if (skip)
                                {
                                    break;
                                }
                                if (seek > 0)
                                {
                                    if(seek < read)
                                    {
                                        read -= seek;
                                        buffer = buffer[(seek - 1)..];
                                    }
                                    else
                                    {
                                        seek -= read;
                                    }
                                    continue;
                                }
                                await discord.WriteAsync(buffer, 0, read);
                                if (state == EStatus.pause)
                                {
                                    bool pauseAgain;
                                    do
                                    {
                                        pauseAgain = await _tcs.Task;
                                        _tcs = new TaskCompletionSource<bool>();
                                    } while (pauseAgain);
                                }
                            }
                            playlist.Remove(track);
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    exit = true;
                }
                catch (Exception ex)
                {
                    exit = true;
                }
                finally
                {
                    state = EStatus.empty;
                    await discord.FlushAsync();
                }
            }
        }
        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
        private string GetNextTrack()
        {
            string path = playlist.FirstOrDefault();
            return path;
        }
        private void ChangeStatus(EStatus state)
        {
            this.state = state;
        }

        #endregion
    }
}
