using discord_bot_csharp.Commands;
using discord_bot_csharp.Ready;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
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
            Console.WriteLine("Please specify a token in the DISCORD_TOKEN environment variable.");
            Environment.Exit(1);
            return;
        }

        config = new()
        {
            Token = token,
            Intents = DiscordIntents.All,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        client = new(config);

        client.Ready += Ready.Ready.OnReady;

        // Commands Next Configuration
        // var commandsConfig = new CommandsNextConfiguration
        // {
        //     EnableMentionPrefix = true,
        //     EnableDms = true,
        //     EnableDefaultHelp = false
        // };

        // Commands = client.UseCommandsNext(commandsConfig);

        // Commands.RegisterCommands<Basic>();

        // Slash Commands Configuration

        var slash = client.UseSlashCommands();

        slash.RegisterCommands<SlashCommands>();

        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await client.ConnectAsync();
        await Task.Delay(-1);
    }
}
