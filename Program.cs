using DSharpPlus;

namespace discord_bot_csharp;

class Program
{
    static async Task Main(string[] args)
    {
        DiscordConfiguration config = new DiscordConfiguration
        {
            Token = Functions.Token.Get(),
            Intents = DiscordIntents.All,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        DiscordClient client = new DiscordClient(config);

        await client.ConnectAsync();

        Loaders.LoadCommands.Load(client);
        await Loaders.LoadServices.Load(client);
        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await Task.Delay(-1);
    }
}
