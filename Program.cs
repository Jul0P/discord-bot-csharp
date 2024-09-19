using discord_bot_csharp.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DotNetEnv;

namespace discord_bot_csharp;

class Program
{
    public static DiscordClient Client { get; set; }
    public static CommandsNextExtension Commands { get; set; }

    static async Task Main(string[] args)
    {
        Env.Load();

        string token = Environment.GetEnvironmentVariable("TOKEN");

        var config = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = token,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        Client = new DiscordClient(config);

        Client.Ready += Client_Ready;

        // Commands Next Configuration
        // var commandsConfig = new CommandsNextConfiguration
        // {
        //     EnableMentionPrefix = true,
        //     EnableDms = true,
        //     EnableDefaultHelp = false
        // };

        // Commands = Client.UseCommandsNext(commandsConfig);

        // Commands.RegisterCommands<Basic>();

        // Slash Commands Configuration

        var slash = Client.UseSlashCommands();

        slash.RegisterCommands<SlashCommands>();

        Console.WriteLine("discord-bot-csharp est actif (J#)");

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
    {
        await Task.Delay(1000);

        var guild = Client.Guilds.Values.FirstOrDefault();
        
        if (guild != null)
        {
            var memberCount = guild.MemberCount;
            var activity = new DiscordActivity
            {
                Name = $"{memberCount} membres",
                ActivityType = ActivityType.Watching
            };

            await Client.UpdateStatusAsync(activity);
        }
    }
}
