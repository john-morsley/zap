using System.ComponentModel.DataAnnotations;

namespace Morsley.UK.ZAP.Common;

public class Display
{
    public static void Title(string value)
    {
        value = value.Trim();

        if (value.Length == 0) return;

        Blank();
        
        var titleWidth = value.Length;
        if (titleWidth < 13)
        {
            titleWidth = 13;
            var padLeft = (13 - value.Length) / 2;
            var padRight = padLeft;
            if ((padLeft + value.Length + padRight) % 2 == 0)
            {
                padRight++;
            }
            value = $"{new string(' ', padLeft)}{value}{new string(' ', padRight)}";
        }

        var numberOfSpaces = 5 + titleWidth + 5;

        var spaces = new string(' ', numberOfSpaces);
        var numberOfSpacesLeftOfTitle = 5;
        var numberOfSpacesRightOfTitle = 5;
        var spacesLeft = new string(' ', numberOfSpacesLeftOfTitle);
        var spacesRight = new string(' ', numberOfSpacesRightOfTitle);

        var numberOfBars = numberOfSpaces - "oOOo-(_)-oOOo".Length;
        var bars = new string('-', numberOfBars);

        var numberOfBarsLeft = (numberOfSpaces - "oOOo-(_)-oOOo".Length) / 2;
        var numberOfBarsRight = numberOfBarsLeft;
        
        if (numberOfBars != numberOfBarsLeft + numberOfBarsRight) numberOfBarsRight++;

        var padding = new string(' ', numberOfBarsLeft + 1);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(@$"{padding}   \\\!///");
        Console.WriteLine(@$"{padding} \\  - -  //");
        Console.Write($"{padding}  (");
        Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write("  @ @  ");
        Console.ForegroundColor = ConsoleColor.White; Console.Write(")\n");

        var barsLeft = new string('-', numberOfBarsLeft);
        var barsRight = new string('-', numberOfBarsRight);

        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"+{barsLeft}");
        Console.ForegroundColor = ConsoleColor.White; Console.Write($"oOOo");
        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"-");
        Console.ForegroundColor = ConsoleColor.White; Console.Write($"(_)");
        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"-");
        Console.ForegroundColor = ConsoleColor.White; Console.Write($"oOOo");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"{barsRight}+\n");
        Console.WriteLine($"!{spaces}!");
        Console.Write($"!");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write($"{spacesLeft}{value}{spacesRight}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"!\n");
        Console.WriteLine($"!{spaces}!");

        barsLeft = new string('-', numberOfBarsLeft + 9);

        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"+{barsLeft}");
        Console.ForegroundColor = ConsoleColor.White; Console.Write($"Oooo");
        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"{barsRight}+\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(@$"{padding}oooO    (   )");
        Console.WriteLine(@$"{padding}(   )    ) /");
        Console.WriteLine(@$"{padding} \ (    (_/");
        Console.WriteLine(@$"{padding}  \_)");

        Console.ResetColor();
        Blank();
    }

    public static void Normal(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    public static void Warning(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    public static void Good(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    public static void Bad(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }

    public static void Blank()
    {
        System.Console.WriteLine();
    }

    public static void Mute(string value)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine(value);
        System.Console.ResetColor();
    }
}