using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace discord_bot_csharp.Commands
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("status", "Envoie un message de status")]
        public async Task TestCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("Le bot est bien actif !");
            await Task.Delay(3000);
            await ctx.DeleteResponseAsync();
        }

        // [SlashCommand("devoir", "Crée un embed pour un devoir")]
        // public async Task DevoirCommand(InteractionContext ctx,
        //     [Option("date", "La date, format: 09/19")] string date,
        //     [Option("groupe", "Le groupe, format: A / B")] string groupe,
        //     [Option("matiere", "La matière, format: anglais, maths, français")] string matiere,
        //     [Option("professeur", "Le professeur, format: maurice")] string professeurs,
        //     [Option("description", "La description")] string description)
        // {
          
        //     var embed = new DiscordEmbedBuilder
        //     {
        //         Title = "Devoir test",
        //         Description = description,
        //         Color = DiscordColor.Blurple
        //     };

          
        //     embed.AddField("Date", date, true);
        //     embed.AddField("Groupe", groupe, true);
        //     embed.AddField("Matière", matiere, true);
        //     embed.AddField("Professeur", professeurs, false);

        
        //     await ctx.CreateResponseAsync(embed: embed.Build());
        // }
    }
}
