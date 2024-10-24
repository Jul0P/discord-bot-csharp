using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Commands;

public class UpdateDevoir : ApplicationCommandModule
{
    private const string FilePath = "src/data/devoir.json";

    [SlashCommand("updatedevoir", "Mettre à jour un devoir")]
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

        bool updated = Update(date, groupe, matiere, description);

        var embed = new DiscordEmbedBuilder
        {
            Title = updated ? "Devoir mis à jour" : "Devoir non trouvé",
            Description = updated ? $"Le devoir du **{date}** pour le groupe **{groupe}** en **{matiere}** a été mis à jour" : "Aucun devoir trouvé",
            Color = updated ? DiscordColor.Green : DiscordColor.Red
        };

        await ctx.CreateResponseAsync(embed: embed.Build());
        await Services.Devoir.Init(ctx.Client, date);
        await Task.Delay(5000);
        await ctx.DeleteResponseAsync();
    }

    private bool Update(string date, string groupe, string matiere, string description)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return false;
        }

        var json = File.ReadAllText(FilePath);
        var devoirs = JsonSerializer.Deserialize<List<DevoirDate>>(json) ?? new List<DevoirDate>();

        var devoirDate = devoirs.FirstOrDefault(d => d.Date == date);
        if (devoirDate == null)
        {
            return false;
        }

        bool updated = false;
        if (groupe == "AB")
        {
            updated = UpdateInGroup(devoirDate, "Groupe A", matiere, description);
            updated = UpdateInGroup(devoirDate, "Groupe B", matiere, description);
        }
        else if (groupe == "A" || groupe == "B")
        {
            updated = UpdateInGroup(devoirDate, "Groupe " + groupe, matiere, description);
        }
        else
        {
            updated = UpdateInGroup(devoirDate, groupe, matiere, description);
        }

        if (updated)
        {
            var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, updatedJson);
        }

        return updated;
    }

    private bool UpdateInGroup(DevoirDate devoirDate, string groupe, string matiere, string description)
    {
        if (!devoirDate.Devoirs.ContainsKey(groupe))
        {
            return false;
        }

        var devoirToUpdate = devoirDate.Devoirs[groupe].FirstOrDefault(d => d.Matiere == matiere);
        if (devoirToUpdate == null)
        {
            return false;
        }

        devoirToUpdate.Description = description;
        return true;
    }
}