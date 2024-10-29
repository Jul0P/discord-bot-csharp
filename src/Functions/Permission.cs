using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Functions;

public class Permission
{
    public static async Task<bool> Get(InteractionContext context, ulong requiredRoleId)
    {
        DiscordMember member = await context.Guild.GetMemberAsync(context.User.Id);
        if (!member.Roles.Any(role => role.Id == requiredRoleId))
        {
            DiscordEmbedBuilder permissionEmbed = new DiscordEmbedBuilder
            {
                Title = "Erreur",
                Description = "Vous n'avez pas la permission d'utiliser cette commande",
                Color = DiscordColor.Red
            };

            await context.CreateResponseAsync(embed: permissionEmbed.Build());
            await Task.Delay(3000);
            await context.DeleteResponseAsync();
            return false;
        }

        return true;
    }
}