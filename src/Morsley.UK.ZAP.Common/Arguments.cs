
namespace Morsley.UK.ZAP.Common;

public static class Arguments
{
    public static void Gather(ref string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Display.Normal($"Please enter the {name}: ");

            value = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                Display.Warning($"{name} cannot be empty.");
            }
        }
    }

    public static async Task<string?> GetJson(string url)
    {
        Display.Blank();
        Display.Mute("Getting JSON...");

        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            Display.Normal($"Fetching JSON from: {url}");

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Display.Good($"Successfully retrieved {json.Length} characters");

            return json;
        }
        catch (Exception)
        {
            Display.Bad("Failed to get JSON!");
            Environment.Exit(1);
        }

        return null;
    }

    public static void Output(string? swaggerUrl)
    {
        if (!string.IsNullOrWhiteSpace(swaggerUrl))
        {
            Display.Normal($"Swagger URL: '{swaggerUrl}'");
        }
    }

    public static bool IsThereAny(string[] args)
    {
        Display.Blank();
        Display.Mute("Do we have arguments passed in?");

        if (!args.Any())
        {
            Display.Bad("No");
            return false;
        }
        else
        {
            Display.Good("Yes");
            return true;
        }
    }

    public static void Process(string[] args, ref string? value, string name)
    {        
        if (args.Any())
        {
            Display.Blank();
            Display.Mute($"Processing arguments, looking for --'{name}'");

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith($"--{name}=", StringComparison.OrdinalIgnoreCase))
                {
                    value = args[i].Substring($"--{name}=".Length);

                    Display.Good($"Found '{name}' --> '{value}'");

                    break;
                }
                else if (args[i].Equals($"--{name}", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 < args.Length)
                    {
                        value = args[i + 1];

                        Display.Good($"Found '{name}' --> {value}");

                        break;
                    }
                }
            }
        }
    }

    public static void Verify(string? swaggerUrl)
    {
        if (string.IsNullOrWhiteSpace(swaggerUrl))
        {
            Display.Bad("Cannot determine the Swagger URL, so exiting...");
            Environment.Exit(1);
        }
    }

    public static void Expected(List<string> expectedArguments, string? forExample)
    {
        Display.Blank();
        Display.Mute("Some expected parameters were missing...");
        foreach (var argument in expectedArguments)
        {
            Display.Normal(argument);
        }
        if (!string.IsNullOrEmpty(forExample))
        {
            Display.Mute($"e.g. {forExample}");
        }
        Display.Bad("Critical: Cannot continue without these parameters!");
        Environment.Exit(1);
    }
}