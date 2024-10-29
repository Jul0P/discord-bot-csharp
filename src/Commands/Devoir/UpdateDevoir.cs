using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Commands;

public class UpdateDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("updatedevoir", "Mettre à jour un devoir")]
    public async Task Command(InteractionContext context,
        [Option("date", "format: 20/10")] string date,
        [Option("groupe", "format: Général / A / B / SLAM / SISR / Maths 2")] string groupe,
        [Option("matiere", "format: CBA, ABL, Maths, Anglais")] string matiere,
        [Option("description", "description")] string description = null,
        [Option("nouvelle_matiere", "format: CBA, ABL, Maths, Anglais")] string nouvelleMatiere = null)
    {
        if (!await Functions.Permission.Get(context, 1280508888206282812))
        {
            return;
        }

        bool updated = Update(date, groupe, matiere, description, nouvelleMatiere);

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = updated ? "Devoir mis à jour" : "Devoir non trouvé",
            Description = updated ? $"Le devoir du **{date}** pour le groupe **{groupe}** en **{(nouvelleMatiere != null ? $"{matiere} ➔ {nouvelleMatiere}" : matiere)}** a été mis à jour" : "Aucun devoir trouvé",
            Color = updated ? DiscordColor.Green : DiscordColor.Red
        };

        await context.CreateResponseAsync(embed: embed.Build());
        await Services.Devoir.Init(context.Client, date);
        await Task.Delay(5000);
        await context.DeleteResponseAsync();
    }

    private bool Update(string date, string groupe, string matiere, string description, string nouvelleMatiere)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return false;
        }

        List<DevoirJour> devoirs = JsonSerializer.Deserialize<List<DevoirJour>>(File.ReadAllText(FilePath)) ?? new List<DevoirJour>();

        DevoirJour devoirDate = devoirs.FirstOrDefault(d => d.Date == date);
        if (devoirDate == null)
        {
            return false;
        }

        bool updated = false;

        if (groupe == "A" || groupe == "B")
        {
            updated = UpdateInGroup(devoirDate, "Groupe " + groupe, matiere, description, nouvelleMatiere);
        }
        else
        {
            updated = UpdateInGroup(devoirDate, groupe, matiere, description, nouvelleMatiere);
        }

        if (updated)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true }));
        }

        return updated;
    }

    private bool UpdateInGroup(DevoirJour devoirDate, string groupe, string matiere, string description, string nouvelleMatiere)
    {
        if (!devoirDate.Devoirs.ContainsKey(groupe))
        {
            return false;
        }

        Devoir devoirToUpdate = devoirDate.Devoirs[groupe].FirstOrDefault(d => d.Matiere == matiere);
        if (devoirToUpdate == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(description))
        {
            devoirToUpdate.Description = description;
        }

        if (!string.IsNullOrEmpty(nouvelleMatiere))
        {
            devoirToUpdate.Matiere = nouvelleMatiere;
        }

        return true;
    }
}