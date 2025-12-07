namespace Morsley.UK.ZAP.Console;

internal class Java
{
    public static void CheckIsRequiredVersion(string? potentialMinimumJavaVersion)
    {
        if (string.IsNullOrEmpty(potentialMinimumJavaVersion))
        {
            Display.Bad("Critical: Missing requiredJavaVersion!");
            Environment.Exit(1);
        }
        
        if (int.TryParse(potentialMinimumJavaVersion, out int minimumJavaVersion))
        {
            CheckIsRequiredVersion(minimumJavaVersion);
        }
        else
        {
            Display.Bad("Critical: Expected requiredJavaVersion to be a number!");
            Environment.Exit(1);
        }
    }

    public static void CheckIsRequiredVersion(int minimumJavaVersion)
    {
        Display.Blank();
        Display.Mute("Checking for Java and version...");

        var javaVersion = GetJavaVersion();
        if (javaVersion == null)
        {
            Display.Warning("Critical: Java is not installed or not in PATH!");
            Display.Normal("Please install Java " + minimumJavaVersion + " or later and ensure it's in your system PATH.");
            Display.Bad("Cannot determine the Swagger URL, so exiting...");
            Environment.Exit(1);
        }

        Display.Normal($"Found Java version: {javaVersion}");

        if (javaVersion.Major < minimumJavaVersion)
        {
            Display.Bad($"Critical: ZAP requires Java {minimumJavaVersion} or later. Found Java {javaVersion.Major}.");
            Environment.Exit(1);
        }
        else
        {
            Display.Good($"Java version adequate.");
        }
    }

    private static Version? GetJavaVersion()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = "-version",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process == null) return null;

            var output = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var versionMatch = System.Text.RegularExpressions.Regex.Match(output, @"version ""?([0-9._]+)");
            if (versionMatch.Success && Version.TryParse(versionMatch.Groups[1].Value, out var version))
            {
                return version;
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    public static void CheckIfJarFileExists(string? jarFolder, string? jarFile)
    {
        Display.Blank();
        Display.Mute("Checking if JAR file exists...");

        try
        {
            var jarPath = Path.Combine(jarFolder, jarFile);
            if (!File.Exists(jarPath))
            {
                Display.Bad($"Critical: Could not find the JAR file!");
                Environment.Exit(1);
            }
            Display.Good($"Found the JAR file.");
        }
        catch (Exception)
        {
            Display.Bad($"Crital: An unexpected exception occurred trying to check for the JAR file!");
            Environment.Exit(1);
        }

    }    
}
