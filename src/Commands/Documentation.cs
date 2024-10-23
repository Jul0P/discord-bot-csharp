using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Linq;
using System.Threading.Tasks;

namespace discord_bot_csharp.Commands;

public class Documentation : ApplicationCommandModule
{
    [SlashCommand("doc", "Documentation du bot")]
    public async Task Command(InteractionContext ctx)
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = "Documentation du Bot",
            Description = "Voici la liste des commandes disponibles et leurs descriptions",
            Color = DiscordColor.Azure
        };

        embed.AddField("\u200B", "**Nécessite le rôle** : <@&1280508888206282812>", false);
        embed.AddField("/adddevoir `date` `groupe` `matiere` `description`", "Ajouter un devoir", false);
        embed.AddField("/deletedevoir `date` `groupe` `matiere`", "Supprimer un devoir", false);
        embed.AddField("/updatedevoir `date` `groupe` `matiere` `description`", "Mettre à jour un devoir", false);
        embed.AddField("/clear `nombre`", "Supprimer des messages", false);

        embed.AddField("\n\u200B", "**Nécessite le rôle** : @everyone", false);
        embed.AddField("/status", "Envoie un message de status", false);
        embed.AddField("/doc", "Documentation du bot", false);

        await ctx.CreateResponseAsync(embed: embed.Build());
    }
}