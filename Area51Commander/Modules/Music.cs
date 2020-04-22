using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Commander.Services;
using System.Threading.Tasks;

namespace Commander.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        private MusicService _musicService;

        public Music(MusicService musicService)
        {
            _musicService = musicService;
        }
        [Command("Join")]
        [Summary("This tells the Music service bot to join the channel.")]
        public async Task Join()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                var builder = new EmbedBuilder()
                {
                    Color = new Color(189, 16, 224),
                    Description = $"```You need to connect to a voice channel.```",
                    Title = "Command"
                };
                builder.WithCurrentTimestamp();
                var Auther = new EmbedAuthorBuilder()
                    .WithName("Commander");
                builder.WithAuthor(Auther);
                await ReplyAsync("",false,builder.Build());
                return;
            }
            else
            {
                var builder = new EmbedBuilder()
                {
                    Color = new Color(189, 16, 224),
                    Description = $"```Now connected to {user.VoiceChannel.Name}```",
                    Title = "Command"
                };
                builder.WithCurrentTimestamp();
                var Auther = new EmbedAuthorBuilder()
                    .WithName("Commander");
                builder.WithAuthor(Auther);
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync("",false,builder.Build());
            }
        }
        [Command("Leave")]
        [Summary("This tells the Music service bot to leave the channel.")]
        public async Task Leave()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                var builder = new EmbedBuilder()
                {
                    Color = new Color(189, 16, 224),
                    Description = $"```Please join the channel the bot is in to make it leave.```",
                    Title = "Command"
                };
                builder.WithCurrentTimestamp();
                var Auther = new EmbedAuthorBuilder()
                    .WithName("Commander");
                builder.WithAuthor(Auther);
                await ReplyAsync("", false, builder.Build());
                return;              
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                var builder = new EmbedBuilder()
                {
                    Color = new Color(189, 16, 224),
                    Description = $"```Bot has now left {user.VoiceChannel.Name}```",
                    Title = "Command"
                };
                builder.WithCurrentTimestamp();
                var Auther = new EmbedAuthorBuilder()
                    .WithName("Commander");
                builder.WithAuthor(Auther);
                await ReplyAsync("", false, builder.Build());
            }
        }

        [Command("Play")]
        [Summary("This tells the Music service bot to play a song that you request.")]
        public async Task Play([Remainder]string query)
            => await ReplyAsync(await _musicService.PlayAsync(query, Context.Guild.Id));

        [Command("Stop")]
        [Summary("This tells the Music service bot to stop playing music.")]
        public async Task Stop()
            => await ReplyAsync(await _musicService.StopAsync(Context.Guild.Id));

        [Command("Skip")]
        [Summary("This tells the Music service bot to skip the current song.")]
        public async Task Skip()
            => await ReplyAsync(await _musicService.SkipAsync(Context.Guild.Id));

        [Command("Volume")]
        [Summary("This tells the Music service bot to set the volume of playing music to a specified interger.")]
        public async Task Volume(int vol)
            => await ReplyAsync(await _musicService.SetVolumeAsync(vol, Context.Guild.Id));

        [Command("Pause")]
        [Summary("This tells the Music service bot to pause playing music.")]
        public async Task Pause()
            => await ReplyAsync(await _musicService.PauseOrResumeAsync(Context.Guild.Id));

        [Command("Resume")]
        [Summary("This tells the Music service bot to resume playing music.")]
        public async Task Resume()
            => await ReplyAsync(await _musicService.ResumeAsync(Context.Guild.Id));
    }
}
