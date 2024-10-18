using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;

namespace discord_bot_csharp.Commands;

public class UpdateDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("updatedevoir", "Met à jour un devoir")]
    public async Task UpdateDevoirCommand(InteractionContext ctx,
        [Option("date", "La date, format: 09/19")] string date,
        [Option("groupe", "Le groupe, format: A / B")] string groupe,
        [Option("matiere", "La matière, format: anglais, maths, français")] string matiere,
        [Option("professeur", "Le professeur, format: maurice")] string professeurs,
        [Option("description", "La description")] string description)
    {
        bool updated = CheckDevoir(date, groupe, matiere, professeurs, description);

        var embed = new DiscordEmbedBuilder
        {
            Title = updated ? "Devoir mis à jour" : "Devoir non trouvé",
            Description = updated ? $"Le devoir du {date} pour le groupe {groupe} en {matiere} a été mis à jour." : "Aucun devoir correspondant trouvé.",
            Color = updated ? DiscordColor.Green : DiscordColor.Red
        };

        await ctx.CreateResponseAsync(embed: embed.Build());
    }

    private bool CheckDevoir(string date, string groupe, string matiere, string professeurs, string description)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return false;
        }

        var json = File.ReadAllText(FilePath);
        var devoirs = JsonSerializer.Deserialize<List<Devoir>>(json) ?? new List<Devoir>();

        var devoirToUpdate = devoirs.FirstOrDefault(d => d.Date == date && d.Groupe == groupe && d.Matiere == matiere);
        if (devoirToUpdate == null)
        {
            return false;
        }

        devoirToUpdate.Professeurs = professeurs;
        devoirToUpdate.Description = description;

        var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, updatedJson);

        return true;
    }
}