namespace Morsley.UK.ZAP.Console;

public static class Zap
{
    public static void Start(string? zapFolder, string? zapJar)
    {
        Display.Blank();
        Display.Mute("Starting ZAP...");


        var zapJarPath = Path.Combine(zapFolder, zapJar);
        if (File.Exists(zapJarPath))
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "java",
                //Arguments = $"-Xmx512m -jar \"{jarPath}\" -daemon",
                Arguments = $"-Xmx512m -jar \"{zapJarPath}\"",
                WorkingDirectory = zapFolder,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };

            // Capture output
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) Display.Mute($"[ZAP] {e.Data}");
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) Display.Warning($"[ZAP Error] {e.Data}");
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Handle process exit
            process.EnableRaisingEvents = true;
            process.Exited += (sender, e) =>
            {
                Display.Normal($"ZAP process exited with code {process.ExitCode}");
                if (process.ExitCode != 0)
                {
                    Display.Bad("ZAP did not exit cleanly! Check the logs above for errors.");
                }
                else
                {
                    Display.Good("ZAP exited cleanly.");
                }
            };
        }
    }
}
