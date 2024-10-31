using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Commands;

public class Documentation : ApplicationCommandModule
{
    [SlashCommand("doc", "Documentation du bot")]
    public async Task Command(InteractionContext context)
    {
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = "Documentation du Bot",
            Description = "Voici la liste des commandes disponibles et leurs descriptions",
            Color = DiscordColor.Azure
        };

        embed.AddField("\u200B", "**Nécessite le rôle** : <@&1280508888206282812> & <@&1280508888206282810>", false);
        embed.AddField("/adddevoir `date` `groupe` `matiere` `description`", "Ajouter un devoir", false);
        embed.AddField("/deletedevoir `date` `groupe` `matiere`", "Supprimer un devoir", false);
        embed.AddField("/updatedevoir `date` `groupe` `matiere` `[description]` `[nouvelle_matiere]`", "Mettre à jour un devoir", false);

        embed.AddField("\n\u200B", "**Nécessite le rôle** : <@&1280508888206282812>", false);
        embed.AddField("/clear `nombre`", "Supprimer des messages", false);

        embed.AddField("\n\u200B", "**Nécessite le rôle** : @everyone", false);
        embed.AddField("/status", "Envoie un message de status", false);
        embed.AddField("/doc", "Documentation du bot", false);

        await context.CreateResponseAsync(embed: embed.Build());
    }
}