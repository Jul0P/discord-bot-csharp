using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace discord_bot_csharp.Commands
{
    public class Basic : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Hello");
        }
    }
}