using DotNetEnv;

namespace discord_bot_csharp.Functions;

public class Token
{
    public static string Get()
    {
        DotNetEnv.Env.Load();
        string token = Environment.GetEnvironmentVariable("TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("Veuillez renseigner le token dans le fichier .env");
            Environment.Exit(1);
        }
        return token;
    }
}