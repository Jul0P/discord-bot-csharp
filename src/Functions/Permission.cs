using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Linq;
using System.Threading.Tasks;

namespace discord_bot_csharp.Functions;

public static class Permission
{
    public static async Task<bool> Get(InteractionContext ctx, ulong requiredRoleId)
    {
        var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);
        if (!member.Roles.Any(role => role.Id == requiredRoleId))
        {
            var permissionEmbed = new DiscordEmbedBuilder
            {
                Title = "Erreur",
                Description = "Vous n'avez pas la permission d'utiliser cette commande",
                Color = DiscordColor.Red
            };

            await ctx.CreateResponseAsync(embed: permissionEmbed.Build());
            await Task.Delay(3000);
            await ctx.DeleteResponseAsync();
            return false;
        }

        return true;
    }
}