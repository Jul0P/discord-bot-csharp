namespace discord_bot_csharp.Models;

public class DevoirDate
{
    public string Date { get; set; }
    public Dictionary<string, List<Devoir>> Devoirs { get; set; }
}