using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Commands;

public class DeleteDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("deletedevoir", "Supprimer un devoir")]
    public async Task Command(InteractionContext context,
        [Option("date", "format: 20/10")] string date,
        [Option("groupe", "format: Général / A / B / SLAM / SISR / Maths 2")] string groupe,
        [Option("matiere", "format: CBA, ABL, Maths, Anglais")] string matiere)
    {
        if (!await Functions.Permission.Get(context, "DeleteDevoir"))
        {
            return;
        }

        bool deleted = await Delete(date, groupe, matiere, context.Client);

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = deleted ? "Devoir supprimé" : "Devoir non trouvé",
            Description = deleted ? $"Le devoir du **{date}** pour le groupe **{groupe}** en **{matiere}** a été supprimé" : "Aucun devoir trouvé",
            Color = deleted ? DiscordColor.Green : DiscordColor.Red
        };

        await context.CreateResponseAsync(embed: embed.Build());
        await Services.Devoir.Init(context.Client, date);
        await Task.Delay(5000);
        await context.DeleteResponseAsync();
    }

    private async Task<bool> Delete(string date, string groupe, string matiere, DiscordClient client)
    {
        List<DevoirJour> devoirs = JsonSerializer.Deserialize<List<DevoirJour>>(File.ReadAllText(FilePath));

        DevoirJour devoirDate = devoirs.FirstOrDefault(d => d.Date == date);

        if (devoirDate == null)
        {
            return false;
        }

        bool removed = false;

        if (groupe == "A" || groupe == "B")
        {
            removed = DeleteFromGroup(devoirDate, "Groupe " + groupe, matiere);
        }
        else
        {
            removed = DeleteFromGroup(devoirDate, groupe, matiere);
        }

        if (devoirDate.Devoirs.Count == 0)
        {
            devoirs.Remove(devoirDate);
            DiscordChannel channel = await client.GetChannelAsync(1297313519825584179);    
            IReadOnlyList<DiscordMessage> messages = await channel.GetMessagesAsync();
            DiscordMessage messageToRemove = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title.Contains(devoirDate.Date)));
            if (messageToRemove != null)
            {
                await messageToRemove.DeleteAsync();
            }
        }

        if (removed)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true }));
        }

        return removed;
    }

    private bool DeleteFromGroup(DevoirJour devoirDate, string groupe, string matiere)
    {
        if (!devoirDate.Devoirs.ContainsKey(groupe))
        {
            return false;
        }

        Devoir devoirToRemove = devoirDate.Devoirs[groupe].FirstOrDefault(d => d.Matiere == matiere);

        if (devoirToRemove == null)
        {
            return false;
        }

        devoirDate.Devoirs[groupe].Remove(devoirToRemove);

        if (devoirDate.Devoirs[groupe].Count == 0)
        {
            devoirDate.Devoirs.Remove(groupe);
        }

        return true;
    }
}