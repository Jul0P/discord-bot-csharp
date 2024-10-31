using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Commands;

public class AddDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("adddevoir", "Ajouter un devoir")]
    public async Task Command(InteractionContext context,
        [Option("date", "format: 20/10")] string date,
        [Option("groupe", "format: Général / A / B / SLAM / SISR / Maths 2")] string groupe,
        [Option("matiere", "format: CBA, ABL, Maths, Anglais")] string matiere,
        [Option("description", "description")] string description)
    {
        if (!await Functions.Permission.Get(context, "AddDevoir"))
        {
            return;
        }
        
        Devoir devoir = new Devoir
        {
            Matiere = matiere,
            Description = description
        };

        List<DevoirJour> devoirs = JsonSerializer.Deserialize<List<DevoirJour>>(File.ReadAllText(FilePath));

        Add(date, groupe, devoir);

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = "Devoir ajouté",
            Description = $"```{description}```",
            Color = DiscordColor.Green
        };

        embed.AddField("Date", date, true);
        embed.AddField("Groupe", groupe, true);
        embed.AddField("Matière", matiere, true);

        await context.CreateResponseAsync(embed: embed.Build());
        await Services.Devoir.Init(context.Client, devoirs.Any(d => d.Date == date) ? date : null);
        await Task.Delay(5000);
        await context.DeleteResponseAsync();
    }

    private void Add(string date, string groupe, Devoir devoir)
    {
        List<DevoirJour> devoirs = JsonSerializer.Deserialize<List<DevoirJour>>(File.ReadAllText(FilePath));

        DevoirJour devoirDate = devoirs.FirstOrDefault(d => d.Date == date);
        
        if (devoirDate == null)
        {
            devoirDate = new DevoirJour { Date = date, Devoirs = new Dictionary<string, List<Devoir>>() };
            devoirs.Add(devoirDate);
        }

        if (groupe == "A" || groupe == "B")
        {
            AddToGroup(devoirDate, "Groupe " + groupe, devoir);
        }
        else
        {
            AddToGroup(devoirDate, groupe, devoir);
        }

        devoirs = devoirs.OrderBy(d => DateTime.ParseExact(d.Date, "dd/MM", CultureInfo.InvariantCulture)).ToList();

        File.WriteAllText(FilePath, JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void AddToGroup(DevoirJour devoirDate, string groupe, Devoir devoir)
    {
        if (!devoirDate.Devoirs.ContainsKey(groupe))
        {
            devoirDate.Devoirs[groupe] = new List<Devoir>();
        }

        devoirDate.Devoirs[groupe].Add(devoir);
    }
}