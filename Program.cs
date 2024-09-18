using discord_bot_csharp.Commands;
using discord_bot_csharp.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace discord_bot_csharp;
class Program
{
    public static DiscordClient Client { get; set; }
    public static CommandsNextExtension Commands { get; set; }
    static async Task Main(string[] args)
    {
        var botConfig = new BotConfig();
        await botConfig.ReadJSON();

        var config = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = botConfig.DiscordBotToken,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        Client = new DiscordClient(config);

        Client.Ready += Client_Ready;

        var commandsConfig = new CommandsNextConfiguration
        {
            StringPrefixes = new string[] { botConfig.DiscordBotPrefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = false
        };

        Commands = Client.UseCommandsNext(commandsConfig);

        Commands.RegisterCommands<Basic>();

        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
    {
        return Task.CompletedTask;
    }
}

