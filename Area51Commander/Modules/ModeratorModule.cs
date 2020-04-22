using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Modules
{

    [Name("Admin Commands")]
    [RequireContext(ContextType.Guild)]
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {

        [Command("Kick")]
        [Summary("Kick the specified user.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, [Remainder] string reason)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(189, 16, 224),
                Description = $"Username: { user.Mention } for {reason}",
                Title = "User Kick Initiated"
            };
            builder.WithCurrentTimestamp();
            var Auther = new EmbedAuthorBuilder()
                .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithFooter($"Initiated by {user.Username}");

            ulong one = 696018470197788715;
            await Context.Guild.GetTextChannel(one).SendMessageAsync("", false, builder.Build());
            await user.Guild.AddBanAsync(user, reason: reason);
            await user.KickAsync();
        }
        [Command("Ban")]
        [Summary("Ban the specific User.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(255, 0, 0),
                Description = $"Username: { user.Mention } for {reason}",
                Title = "User Ban Initiated"
            };
            builder.WithCurrentTimestamp();
            var Auther = new EmbedAuthorBuilder()
                .WithName("Area51 Commander");
            builder.WithAuthor(Auther);
            builder.WithFooter($"Initiated by {user.Username}");

            ulong one = 696018470197788715;
            await Context.Guild.GetTextChannel(one).SendMessageAsync("", false, builder.Build());
            await user.Guild.AddBanAsync(user, reason: reason);
        }
        [Command("Purge")]
        [Summary("Downloads and removes X messages from the current channel.")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int amount)
        {
            if (amount <= 0)
            {
                await ReplyAsync("The amount of messages to remove must be positive.");
                return;
            }
            var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();
            var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 5);
            var count = filteredMessages.Count();
            if (count == 0)
                await ReplyAsync("Nothing to delete.");
            else
            {
                await (Context.Channel as ITextChannel).DeleteMessagesAsync(filteredMessages);
                await ReplyAsync($"Done. Removed {count} {(count > 1 ? "messages" : "message")}.");
            }
        }
        [Command("ann")]
        [Summary("Sends a message to the Announcements Channel!")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public Task Say([Remainder]string text)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = text
            };
            var Auther = new EmbedAuthorBuilder()
    .WithName("Commander");

            builder.WithAuthor(Auther);
            ulong one = 702473688431329380;
            Context.Guild.GetTextChannel(one).SendMessageAsync("", false, builder.Build());
            return ReplyAsync("Post preview:", false, builder.Build());
        }


    }
}