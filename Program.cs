using DSharpPlus;
using DSharpPlus.SlashCommands;
using DotNetEnv;

namespace discord_bot_csharp;

class Program
{
    private static DiscordClient client { get; set; }
    private static DiscordConfiguration config { get; set; }
    private static string token { get; set; }


    static async Task Main(string[] args)
    {
        DotNetEnv.Env.Load();
        token = Environment.GetEnvironmentVariable("TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("Veuillez renseigner le token dans le fichier .env");
            Environment.Exit(1);
            return;
        }

        config = new DiscordConfiguration
        {
            Token = token,
            Intents = DiscordIntents.All,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        client = new DiscordClient(config);

        client.Ready += Ready.Ready.OnReady;

        Loaders.LoadCommands.Load(client);

        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await client.ConnectAsync();
        await Task.Delay(-1);
    }
}
