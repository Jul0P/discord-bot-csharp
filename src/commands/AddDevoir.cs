using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;

namespace discord_bot_csharp.Commands;

public class AddDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("adddevoir", "Crée un embed pour un devoir")]
    public async Task AddDevoirCommand(InteractionContext ctx,
        [Option("date", "La date, format: 09/19")] string date,
        [Option("groupe", "Le groupe, format: A / B")] string groupe,
        [Option("matiere", "La matière, format: anglais, maths, français")] string matiere,
        [Option("professeur", "Le professeur, format: maurice")] string professeurs,
        [Option("description", "La description")] string description)
    {
        var devoir = new Devoir
        {
            Date = date,
            Groupe = groupe,
            Matiere = matiere,
            Professeurs = professeurs,
            Description = description
        };

        SaveDevoir(devoir);

        var embed = new DiscordEmbedBuilder
        {
            Title = "Devoir ajouté",
            Description = description,
            Color = DiscordColor.Blurple
        };

        embed.AddField("Date", date, true);
        embed.AddField("Groupe", groupe, true);
        embed.AddField("Matière", matiere, true);
        embed.AddField("Professeur", professeurs, true);

        await ctx.CreateResponseAsync(embed: embed.Build());
    }

    private void SaveDevoir(Devoir devoir)
    {
        List<Devoir> devoirs;

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            File.WriteAllText(FilePath, "[]");
        }

        var json = File.ReadAllText(FilePath);
        devoirs = JsonSerializer.Deserialize<List<Devoir>>(json) ?? new List<Devoir>();

        devoirs.Add(devoir);
        var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, updatedJson);
    }
}

public class Devoir
{
    public string Date { get; set; }
    public string Groupe { get; set; }
    public string Matiere { get; set; }
    public string Professeurs { get; set; }
    public string Description { get; set; }
}