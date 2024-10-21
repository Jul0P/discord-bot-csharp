using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;

namespace discord_bot_csharp.Commands;

public class DeleteDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("deletedevoir", "Supprime un devoir")]
    public async Task Command(InteractionContext ctx,
        [Option("date", "La date, format: 09/19")] string date,
        [Option("groupe", "Le groupe, format: A / B")] string groupe,
        [Option("matiere", "La matière, format: anglais, maths, français")] string matiere)
    {
        bool deleted = RemoveDevoir(date, groupe, matiere);

        var embed = new DiscordEmbedBuilder
        {
            Title = deleted ? "Devoir supprimé" : "Devoir non trouvé",
            Description = deleted ? $"Le devoir du {date} pour le groupe {groupe} en {matiere} a été supprimé." : "Aucun devoir correspondant trouvé.",
            Color = deleted ? DiscordColor.Green : DiscordColor.Red
        };

        await ctx.CreateResponseAsync(embed: embed.Build());
    }

    private bool RemoveDevoir(string date, string groupe, string matiere)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return false;
        }

        var json = File.ReadAllText(FilePath);
        var devoirs = JsonSerializer.Deserialize<List<Devoir>>(json) ?? new List<Devoir>();

        var devoirToRemove = devoirs.FirstOrDefault(d => d.Date == date && d.Groupe == groupe && d.Matiere == matiere);
        if (devoirToRemove == null)
        {
            return false;
        }

        devoirs.Remove(devoirToRemove);
        var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, updatedJson);

        return true;
    }
}