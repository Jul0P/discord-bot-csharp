using DSharpPlus;
using DSharpPlus.Entities;
using discord_bot_csharp.Models;
using System.Globalization;
using System.Text.Json;

namespace discord_bot_csharp.Services;

public static class Devoir
{ 
    private const string FilePath = "src/data/devoir.json";

    public static async Task Init(DiscordClient client, string dateToProcess = null)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return;
        }

        var json = File.ReadAllText(FilePath);
        var devoirs = JsonSerializer.Deserialize<List<DevoirDate>>(json) ?? new List<DevoirDate>();
        var channel = await client.GetChannelAsync(1297313519825584179);
        var messages = await channel.GetMessagesAsync();

        foreach (var devoirDate in devoirs)
        {
            if (devoirDate.Devoirs == null || (dateToProcess != null && devoirDate.Date != dateToProcess))
            {
                continue;
            }

            var date = DateTime.ParseExact(devoirDate.Date, "dd/MM", CultureInfo.InvariantCulture);
            var dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetDayName(date.DayOfWeek);
            dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").TextInfo.ToTitleCase(dayOfWeek);

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{dayOfWeek} {devoirDate.Date}",
                Color = DiscordColor.DarkBlue
            };

            foreach (var group in new[] { "Groupe A", "Groupe B", "SLAM", "SISR" })
            {
                if (devoirDate.Devoirs.ContainsKey(group))
                {
                    var groupContent = string.Join("\n", devoirDate.Devoirs[group].Select(d => $"**{d.Matiere}** : {d.Description}"));
                    embed.AddField($"**{group}**", groupContent);
                }
            }

            var existingMessage = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title == embed.Title));
            if (existingMessage != null)
            {
                await existingMessage.ModifyAsync(embed: embed.Build());
            }
            else
            {
                await channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}