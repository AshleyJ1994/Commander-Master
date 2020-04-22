using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Microsoft.Extensions.Configuration;
using System.IO;
using Area51Commander.Services;
using Area51Commander.Entities;

namespace Area51Commander
{
    public class CommandHandler
    {

        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private IServiceProvider _services;
        private IConfigurationRoot _config;

        public readonly string _prefix = "$";


        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services, IConfigurationRoot config)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
            _config = config;
       }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += OnMessageReceivedAsync;
            _client.MessageReceived += BadWordsWarn;
            _client.Ready += ConnectedAsync;
        }
        private async Task ConnectedAsync()
        {
            ulong id = 678241085180346368;
            var IMessageChannel = _client.GetChannel(id) as IMessageChannel;
            var Status = _client.SetGameAsync("Area51 Administration", "", ActivityType.Playing);
            await Status;
            var vis = _client.SetStatusAsync(UserStatus.Online);
            await vis;
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = "```The current version of the bot that this discord is running is: Version 0.1.0```",
                Title = "BOT ONLINE",
                ThumbnailUrl = "https://beta.area51platoon.co.uk/wp-content/themes/ae51/res/AE51FULL.svg"
            };
            builder.WithAuthor("Area51 Commander");
            builder.WithFooter("Created by Ashley Johnson - 1¡LuCkY√#5492");
            await IMessageChannel.SendMessageAsync("", false, builder.Build());
        }
        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            // Ensure the message is from a user/bot
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Author.Id == _client.CurrentUser.Id) return;     // Ignore self when checking commands
            var context = new SocketCommandContext(_client, msg);     // Create the command context
            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix("$", ref argPos))   // || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmdService.ExecuteAsync(context, argPos, _services);     // Execute the command
                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
        private async Task<Task> BadWordsWarn(SocketMessage msg)
        {
            string[] badWords = File.ReadAllLines(@"bad-words.txt");
            foreach (string badWord in badWords)
            {
                if (msg.Content.Replace(" ", "").ToLower().Contains(badWord.Replace(" ", "")))
                {
                    await msg.DeleteAsync();
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
     
        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
