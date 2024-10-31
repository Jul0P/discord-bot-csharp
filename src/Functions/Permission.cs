using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Functions;

public class Permission
{
    // 1280508888206282812 = @Admin
    // 1280508888206282810 = @Scribe
    public static Dictionary<string, List<ulong>> CommandPermissions = new Dictionary<string, List<ulong>>
    {
        { "AddDevoir", new List<ulong> { 1280508888206282812, 1280508888206282810 } },
        { "DeleteDevoir", new List<ulong> { 1280508888206282812, 1280508888206282810 } },
        { "UpdateDevoir", new List<ulong> { 1280508888206282812, 1280508888206282810 } },
        { "Clear", new List<ulong> { 1280508888206282812 } }
    };

    public static async Task<bool> Get(InteractionContext context, string commandName)
    {
        List<ulong> requiredRoles = CommandPermissions[commandName];
        DiscordMember member = await context.Guild.GetMemberAsync(context.User.Id);
        if (!member.Roles.Any(role => requiredRoles.Contains(role.Id)))
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