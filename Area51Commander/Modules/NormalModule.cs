using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Audio;
using Commander.Modules;
using Commander.Entities;
using Commander.Services;
using System.Net.Http;
using System;
using HtmlAgilityPack;
using System.Linq;

namespace Commander.Modules
{
    
    [Name("Echo Command!")]
    public class AudioService : ModuleBase<SocketCommandContext>
    {
        public readonly DiscordSocketClient _client;
        [Command("Say"), Alias("s")]
        [Summary("Make the bot say something")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task Say([Remainder]string text)
        {
            return ReplyAsync(text);
        }

        [Command("version")]
        [Summary("Version of the bot")]
        public async Task Version()
        {
           
         
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = "```The current version of the bot that this discord is running is: Version 1.1.0```",
                Title = "Discord Bot Version",
            };
            var Auther = new EmbedAuthorBuilder()
                .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithFooter("Created by Ashley Johnson - 1¡LuCkY√#5492");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("report"), Alias("helpme")]
        [Summary("Contacts Admin for help")]
        public async Task IngameHelp([Remainder] string text)
        {

            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Description = $"{text}",
                Title = $"Hello, there is currently an open report from {Context.Message.Author}",
            };
            var Auther = new EmbedAuthorBuilder()
    .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithCurrentTimestamp();
            ulong id = 702517666560081940;
            await Context.Guild.GetTextChannel(id).SendMessageAsync("", false, builder.Build());
        }

        [Command("Servers")]
        [Summary("Gets the stats for Servers")]
        public async Task BattleField()
        {
            var html = @"https://battlelog.battlefield.com/bf4/servers/show/pc/6ac6e69f-d76d-49b6-a15f-155fbc50fefd/RPG-Heros-House-Of-Fun/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='server-page-info']/div[2]/div[1]/section/h5"); 

           
            var builder = new EmbedBuilder()
            {
                Color = new Color(189, 16, 224),
                Description = $"",
                Title = "Server Population",               
            };
            var Auther = new EmbedAuthorBuilder()
                .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithCurrentTimestamp();
            builder.AddField(x =>
            {
                x.Name = "RPG Heros House Of Fun";
                x.Value = node.InnerText;
                x.IsInline = true;
            });
           
            await ReplyAsync("", false, builder.Build());
        }
        [Command("Streams")]
        [Summary("Get a list of steamers")]
        public async Task Streams()
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(189, 16, 224),
                Description = $"",
                Title = "Streamers on the Discord!",
            };
            var Auther = new EmbedAuthorBuilder()
             .WithName("Commander");
            builder.WithCurrentTimestamp();
            builder.AddField(x =>
            {
                x.Name = "LuCkY";
                x.Value = "https://www.youtube.com/channel/UCwhAh3kIej3lz8u8rdVCoEA?";
                x.IsInline = true;
            });
            await ReplyAsync("", false, builder.Build());
        }
    }


}