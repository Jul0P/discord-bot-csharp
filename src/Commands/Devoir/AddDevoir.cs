using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Commands;

public class AddDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("adddevoir", "Ajouter un devoir")]
    public async Task Command(InteractionContext ctx,
        [Option("date", "format: 20/10")] string date,
        [Option("groupe", "format: A / B / SLAM / SISR / AB")] string groupe,
        [Option("matiere", "format: CBA, ABL, Maths, Anglais")] string matiere,
        [Option("description", "description")] string description)
    {
        if (!await Functions.Permission.Get(ctx, 1280508888206282812))
        {
            return;
        }
        
        var devoir = new Devoir
        {
            Matiere = matiere,
            Description = description
        };

        Add(date, groupe, devoir);

        var embed = new DiscordEmbedBuilder
        {
            Title = "Devoir ajouté",
            Description = $"```{description}```",
            Color = DiscordColor.Green
        };

        embed.AddField("Date", date, true);
        embed.AddField("Groupe", groupe, true);
        embed.AddField("Matière", matiere, true);

        await ctx.CreateResponseAsync(embed: embed.Build());
        await Services.Devoir.Init(ctx.Client, date);
        await Task.Delay(5000);
        await ctx.DeleteResponseAsync();
    }

    private void Add(string date, string groupe, Devoir devoir)
    {
        List<DevoirDate> devoirs;

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            File.WriteAllText(FilePath, "[]");
        }

        var json = File.ReadAllText(FilePath);
        devoirs = JsonSerializer.Deserialize<List<DevoirDate>>(json) ?? new List<DevoirDate>();

        var devoirDate = devoirs.FirstOrDefault(d => d.Date == date);
        if (devoirDate == null)
        {
            devoirDate = new DevoirDate { Date = date, Devoirs = new Dictionary<string, List<Devoir>>() };
            devoirs.Add(devoirDate);
        }

        if (groupe == "AB")
        {
            AddToGroup(devoirDate, "Groupe A", devoir);
            AddToGroup(devoirDate, "Groupe B", devoir);
        }
        else if (groupe == "A" || groupe == "B")
        {
            AddToGroup(devoirDate, "Groupe " + groupe, devoir);
        }
        else
        {
            AddToGroup(devoirDate, groupe, devoir);
        }

        devoirs = devoirs.OrderBy(d => DateTime.ParseExact(d.Date, "dd/MM", CultureInfo.InvariantCulture)).ToList();

        var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, updatedJson);
    }

    private void AddToGroup(DevoirDate devoirDate, string groupe, Devoir devoir)
    {
        if (!devoirDate.Devoirs.ContainsKey(groupe))
        {
            devoirDate.Devoirs[groupe] = new List<Devoir>();
        }

        devoirDate.Devoirs[groupe].Add(devoir);
    }
}