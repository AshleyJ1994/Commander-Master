using Commander.Entities;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        public HelpModule(CommandService service)
        {
            _service = service;
        }
        [Command("Help")]
        [Summary("This help box!")]
        public async Task HelpAsync()
        {
            string prefix = "$";
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "Here is a list of my available commands",
                Title = "Help",

            };
            var Auther = new EmbedAuthorBuilder()
                .WithName("Commander");
            builder.WithAuthor(Auther);
            builder.WithFooter("Created by Ashley Johnson - 1¡LuCkY√#5492");

            foreach (var module in _service.Modules)
            {
                string description = null;
                string Summary = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"```{prefix}{cmd.Aliases.First()}```" + $" {cmd.Summary}\n";
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description + Summary;
                        x.IsInline = false;
                    });
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

    }
}

