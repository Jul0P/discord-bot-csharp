using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp;

class Program
{
    private static DiscordClient Client { get; set; }
    private static DiscordConfiguration Config { get; set; }

    static async Task Main(string[] args)
    {
        Config = new DiscordConfiguration
        {
            Token = Functions.Token.Get(),
            Intents = DiscordIntents.All,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        Client = new DiscordClient(Config);

        await Client.ConnectAsync();

        Loaders.LoadCommands.Load(Client);
        await Loaders.LoadServices.Load(Client);
        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await Task.Delay(-1);
    }
}
