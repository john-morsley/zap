

namespace Morsley.UK.ZAP.Console;

internal static class Templater
{    
    // Add these constants at the top with your other constants
    private const string VariablesFolder = "variables";
    private const string VariablesFile = "general.json";

    public static bool ProcessTemplateFile(string templatePath, Dictionary<string, string> replacements)
    {
        try
        {
            var templateContent = File.ReadAllText(templatePath);
            var processedContent = Regex.Replace(templateContent,
                @"\[\[(.*?)\]\]",
                match => replacements.TryGetValue(match.Groups[1].Value, out var replacement)
                    ? replacement
                    : match.Value);

            var outputPath = Path.Combine(
                Path.GetDirectoryName(templatePath),
                Path.GetFileNameWithoutExtension(templatePath).Replace("-template", "-actual") + ".yaml");

            File.WriteAllText(outputPath, processedContent);

            return true;
        }
        catch
        {

            return false;
        }




        //return outputPath;
    }

    // Add this method to your Program class
    static Dictionary<string, string> LoadVariables()
    {
        var variablesPath = Path.Combine(VariablesFolder, VariablesFile);
        if (!File.Exists(variablesPath))
        {
            throw new FileNotFoundException($"Variables file not found: {variablesPath}");
        }

        var json = File.ReadAllText(variablesPath);
        var variables = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        return variables ?? new Dictionary<string, string>();
    }



    //var variables = LoadVariables();
}