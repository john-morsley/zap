namespace Morsley.UK.ZAP.Console;

public class Plan
{
    public static string Create(string planFolder, string templatePlanFilename)
    {
        Display.Blank();
        Display.Mute("Creating a plan from template...");

        var planTemplatePath = Path.Combine(planFolder, templatePlanFilename);
        if (!File.Exists(planTemplatePath))
        {
            Display.Bad("Critical: Template file does not exist.");
            Environment.Exit(1);
            return null;
        }
        else
        {
            Display.Normal("Template file exists.");

            var templateContent = File.ReadAllText(planTemplatePath);
            return templateContent;
        }
    }
}
