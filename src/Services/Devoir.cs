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
        var currentDate = DateTime.Now;

        messages = messages.Except(messages.Where(m => m.Embeds.Any(e => e.Title.Contains("Devoir")))).Reverse().ToList();

        int messageIndex = 0;
        bool devoirsUpdated = false;

        var devoirsToRemove = new List<DevoirDate>();

        foreach (var devoirDate in devoirs)
        {
            if (devoirDate.Devoirs == null || (dateToProcess != null && devoirDate.Date != dateToProcess))
            {
                continue;
            }

            var date = DateTime.ParseExact(devoirDate.Date, "dd/MM", CultureInfo.InvariantCulture);
            var dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetDayName(date.DayOfWeek);
            dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").TextInfo.ToTitleCase(dayOfWeek);
            var dayDifference = (currentDate - date).Days;

            if (dayDifference > 4)
            {
                devoirsToRemove.Add(devoirDate);
                devoirsUpdated = true;

                var messageToRemove = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title == $"{dayOfWeek} {devoirDate.Date}"));
                if (messageToRemove != null)
                {
                    await messageToRemove.DeleteAsync();
                    messages = await channel.GetMessagesAsync();
                    messages = messages.Except(messages.Where(m => m.Embeds.Any(e => e.Title.Contains("Devoir")))).Reverse().ToList();
                }
                continue;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{dayOfWeek} {devoirDate.Date}",
                Color = dayDifference > 0 ? DiscordColor.Orange : DiscordColor.DarkBlue
            };

            foreach (var group in new[] { "Général", "Groupe A", "Groupe B", "SLAM", "SISR", "Maths 2" })
            {
                if (devoirDate.Devoirs.ContainsKey(group))
                {
                    var groupContent = string.Join("\n", devoirDate.Devoirs[group].Select(d => $"**{d.Matiere}** : {d.Description}"));
                    embed.AddField($"**{group}**", groupContent);
                }
            }

            var existingMessage = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title == embed.Title));
            if (existingMessage != null && dateToProcess != null)
            {
                await existingMessage.ModifyAsync(embed: embed.Build());
            }
            else if (messageIndex < messages.Count && dateToProcess == null)
            {
                var Message = messages[messageIndex];
                var existingEmbedTitle = Message.Embeds.FirstOrDefault()?.Title;
                if (existingEmbedTitle != embed.Title)
                {
                    await Message.ModifyAsync(embed: embed.Build());
                }
            }
            else
            {
                await channel.SendMessageAsync(embed: embed.Build());
            }

            messageIndex++;
        }

        foreach (var devoirDate in devoirsToRemove)
        {
            devoirs.Remove(devoirDate);
        }

        if (devoirsUpdated)
        {
            var updatedJson = JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, updatedJson);
        }
    }
}