try
{
    Display.Title("Morsley.UK - ZAP Launcher");

    string? zapFolder = null;
    string? zapJar = null;
    string? requiredJavaVersion = null;
    string? headless = "true";
    string? planFolder = null;
    string? templatePlanFilename = null;

    if (Arguments.IsThereAny(args))
    {
        Arguments.Process(args, ref zapFolder, "zapFolder");
        Arguments.Process(args, ref zapJar, "zapJar");
        Arguments.Process(args, ref requiredJavaVersion, "requiredJavaVersion");
        Arguments.Process(args, ref headless, "headless");
        Arguments.Process(args, ref planFolder, "planFolder");
        Arguments.Process(args, ref templatePlanFilename, "templatePlanFilename");
    }
    else
    {
        var expectedArguments = new List<string> { "--zapFolder", "--zapJar", "--requiredJavaVersion", "--headless", "--planFolder", "--templatePlanFilename" };
        var forExample = 
            "--zapFolder \"C:\\Program Files\\ZAP\\Zed Attack Proxy\" " +
            "--zapJar \"zap-2.16.1.jar\" " +
            "--requiredJavaVersion 11 " +
            "--headless true " +
            "--planFolder \"C:\\Development\\GitHub\\zap\\src\\Morsley.UK.ZAP.Console\\plans\"" +
            "--templatePlanFilename \"zap-automation-plan-with-authentication-template.yaml\";";
        Arguments.Expected(expectedArguments, forExample);
    }

    Java.CheckIsRequiredVersion(requiredJavaVersion);
    Java.CheckIfJarFileExists(zapFolder, zapJar);

    var createdPlanFilename = Plan.Create(planFolder, templatePlanFilename);







    //Zap.Start(zapFolder, zapJar);

    Console.ReadKey();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"An error occurred: {ex.Message}");
    Environment.Exit(1);
}