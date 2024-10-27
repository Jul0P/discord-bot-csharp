using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;
using discord_bot_csharp.Functions;

namespace discord_bot_csharp.Commands;

public class AddDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("adddevoir", "Ajouter un devoir")]
    public async Task Command(InteractionContext ctx,
        [Option("date", "format: 20/10")] string date,
        [Option("groupe", "format: Général / A / B / SLAM / SISR / Maths 2")] string groupe,
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

        var dateExists = DateExists(date);

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

        if (dateExists)
        {
            await Services.Devoir.Init(ctx.Client, date);
        }
        else
        {
            await Services.Devoir.Init(ctx.Client);
        }
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

        if (groupe == "A" || groupe == "B")
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

    private bool DateExists(string date)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return false;
        }

        var json = File.ReadAllText(FilePath);
        var devoirs = JsonSerializer.Deserialize<List<DevoirDate>>(json) ?? new List<DevoirDate>();

        return devoirs.Any(d => d.Date == date);
    }
}