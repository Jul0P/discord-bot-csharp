using DSharpPlus;
using DSharpPlus.Entities;
using System.Text.Json;
using System.IO;
using System.Globalization;
using discord_bot_csharp.Models;

namespace discord_bot_csharp.Services;

public class Devoir
{ 
    private const string FilePath = "src/data/devoir.json";

    public static async Task Init(DiscordClient client, string dateToProcess = null)
    {
        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return;
        }

        List<DevoirJour> devoirs = JsonSerializer.Deserialize<List<DevoirJour>>(File.ReadAllText(FilePath)) ?? new List<DevoirJour>();
        DiscordChannel channel = await client.GetChannelAsync(1297313519825584179);
        IReadOnlyList<DiscordMessage> messages = await channel.GetMessagesAsync();
        DateTime currentDate = DateTime.Now;

        messages = messages.Except(messages.Where(m => m.Embeds.Any(e => e.Title.Contains("Devoir")))).Reverse().ToList();

        int messageIndex = 0;
        bool devoirsUpdated = false;

        List<DevoirJour> devoirsToRemove = new List<DevoirJour>();

        foreach (DevoirJour devoirDate in devoirs)
        {
            if (devoirDate.Devoirs == null || (dateToProcess != null && devoirDate.Date != dateToProcess))
            {
                continue;
            }

            DateTime date = DateTime.ParseExact(devoirDate.Date, "dd/MM", CultureInfo.InvariantCulture);
            string dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetDayName(date.DayOfWeek);
            dayOfWeek = CultureInfo.GetCultureInfo("fr-FR").TextInfo.ToTitleCase(dayOfWeek);
            int dayDifference = (currentDate - date).Days;

            if (dayDifference > 4)
            {
                devoirsToRemove.Add(devoirDate);
                devoirsUpdated = true;

                DiscordMessage messageToRemove = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title == $"{dayOfWeek} {devoirDate.Date}"));
                if (messageToRemove != null)
                {
                    await messageToRemove.DeleteAsync();
                    messages = await channel.GetMessagesAsync();
                    messages = messages.Except(messages.Where(m => m.Embeds.Any(e => e.Title.Contains("Devoir")))).Reverse().ToList();
                }
                continue;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{dayOfWeek} {devoirDate.Date}",
                Color = dayDifference > 0 ? DiscordColor.Orange : DiscordColor.DarkBlue
            };

            foreach (string group in new[] { "Général", "Groupe A", "Groupe B", "SLAM", "SISR", "Maths 2" })
            {
                if (devoirDate.Devoirs.ContainsKey(group))
                {
                    string groupContent = string.Join("\n", devoirDate.Devoirs[group].Select(d => $"**{d.Matiere}** : {d.Description}"));
                    embed.AddField($"**{group}**", groupContent);
                }
            }

            DiscordMessage existingMessage = messages.FirstOrDefault(m => m.Embeds.Any(e => e.Title == embed.Title));
            if (existingMessage != null && dateToProcess != null)
            {
                await existingMessage.ModifyAsync(embed: embed.Build());
            }
            else if (messageIndex < messages.Count && dateToProcess == null)
            {
                DiscordMessage Message = messages[messageIndex];
                string existingEmbedTitle = Message.Embeds.FirstOrDefault()?.Title;
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

        foreach (DevoirJour devoirDate in devoirsToRemove)
        {
            devoirs.Remove(devoirDate);
        }

        if (devoirsUpdated)
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(devoirs, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}