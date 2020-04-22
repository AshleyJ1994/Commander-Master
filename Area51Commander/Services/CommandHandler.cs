using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Commander.Services;
using System.IO;
using System.Linq;

namespace Commander
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;
        private readonly string _prefix = "$";

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
        }
        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += OnMessageReceivedAsync;
            _client.MessageReceived += BadWordsWarn;
            _client.Ready += ConnectedAsync;
            _client.UserJoined += AnnounceJoinedUser;
            _client.UserLeft += AnnounceUserLeft;
        }
        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            // Ensure the message is from a user/bot
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Author.Id == _client.CurrentUser.Id) return;     // Ignore self when checking commands
            var context = new SocketCommandContext(_client, msg);     // Create the command context
            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmdService.ExecuteAsync(context, argPos, _services);     // Execute the command
                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
        public async Task AnnounceJoinedUser(SocketGuildUser user) 
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = "```welcome to the RPG Commuinity! please take some time to read our rules and enjoy your time here```",
                Title = "Welcome!",
            };
            var Auther = new EmbedAuthorBuilder()
               .WithName("Commander");
            builder.WithAuthor(Auther);
            await Discord.UserExtensions.SendMessageAsync(user, null , false, builder.Build());

            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "RPG Members");
            await (user as IGuildUser).AddRoleAsync(role);
        }

        public async Task AnnounceUserLeft(IGuildUser user)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = $"```User {user.Nickname}{user.Username} has left the discord```",
                Title = "User Left"
            };
            var Auther = new EmbedAuthorBuilder()
               .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithCurrentTimestamp();
            ulong id = 696018470197788715;
            var IMessageChannel = _client.GetChannel(id) as IMessageChannel;
            await IMessageChannel.SendMessageAsync("", false, builder.Build());
        }
        private async Task ConnectedAsync()
        {
            ulong id = 696018470197788715;
            var IMessageChannel = _client.GetChannel(id) as IMessageChannel;
            var Status = _client.SetGameAsync("RPG Administration", "", ActivityType.Playing);
            await Status;
            var vis = _client.SetStatusAsync(UserStatus.Online);
            await vis;
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = "```Bot Activated```",
                Title = "BOT ONLINE",
            };
            var Auther = new EmbedAuthorBuilder()
                .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithFooter("Created by Ashley Johnson - 1¡LuCkY√#5492");
            await IMessageChannel.SendMessageAsync("", false, builder.Build());
        }

        private async Task<Task> BadWordsWarn(SocketMessage msg)
        {
            ulong id = 696018470197788715;
            var IMessageChannel = _client.GetChannel(id) as IMessageChannel;
            string[] badWords = File.ReadAllLines(@"bad-words.txt");
            foreach (string badWord in badWords)
            {
                if (msg.Content.Replace(" ", "").ToLower().Contains(badWord.Replace(" ", "")))
                {

                    var builder = new EmbedBuilder()
                    {
                        Color = new Color(0, 255, 0),
                        Description = $"```This message was removed due to foul language: '{msg}' because of the word: '{badWord}'```",
                        Title = $"Message Deleted from {msg.Author}",
                    };
                    
                    await msg.DeleteAsync();
                    await IMessageChannel.SendMessageAsync("", false, builder.Build());
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}