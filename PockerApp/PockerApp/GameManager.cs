namespace PockerApp;

internal class GameManager
{
    private static int Width;
    private static int Height;
    private static char[,] array;

    public static void InitDisplay()
    {
        Console.WriteLine("Ce programme doit être lancé en mode plein écran afin de fonctionner sans erreur.");
        Console.WriteLine("Merci d'appuyer sur entrée lorsque la fenêtre est en mode plein écran");
        Console.ReadLine();
        Console.Clear();
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
        array = new char[Height, Width];
        var text = @"''
        ' (                                                                                                               '
        ' )\ )              )                 )                                            (                           )  '
        '(()/(           ( /(    (   (     ( /(  (         )       )     )      )          )\ )   (       )         ( /(  '
        ' /(_)) (    (   )\())  ))\  )(    )\()) )\ )     (     ( /(  ( /(   ( /(   (     (()/(   )(   ( /(  `  )   )\()) '
        '(_))   )\   )\ ((_)\  /((_)(()\  ((_)\ (()/(     )\  ' )(_)) )\())  )(_))  )\ )   ((_)) (()\  )(_)) /(/(  ((_)\  '
        '| _ \ ((_) ((_)| |(_)(_))   ((_) | |(_) )(_))  _((_)) ((_)_ ((_)\  ((_)_  _(_/(   _| |   ((_)((_)_ ((_)_\ | |(_) '
        '|  _// _ \/ _| | / / / -_) | '_| | '_ \| || | | '  \()/ _` |\ \ /  / _` || ' \))/ _` |  | '_|/ _` || '_ \)| ' \  '
        '|_|  \___/\__| |_\_\ \___| |_|   |_.__/ \_, | |_|_|_| \__,_|/_\_\  \__,_||_||_| \__,_|  |_|  \__,_|| .__/ |_||_| '
        '                                        |__/                                                       |_|           '
        ''";
        Console.WriteLine(text);
    }

    public static void PrintRound(List<Cartes> cardList, int number, Player player)
    {
        ResetDisplay();
        PrintTable(cardList, number);
        PrintPlayerCard(player);
        UpdateScreen();
    }

    public static void PrintTable(List<Cartes> cardList, int number)
    {
        var decalage = -number / 2;
        for (var i = 0; i < number; i++)
        {
            AddTextToArray(Height / 2 - 3, Width / 2 - 3 + decalage * 10, CardToString(cardList[i]));
            decalage++;
        }
    }

    public static void PrintPlayerCard(Player player)
    {
        AddTextToArray(Height / 2 + 10, Width / 2 - 6, CardToString(player.Card1!));
        AddTextToArray(Height / 2 + 10, Width / 2 + 6, CardToString(player.Card2!));
    }

    private static void AddTextToArray(int x, int y, string text)
    {
        var tmp = y;
        foreach (var chr in text)
            if (chr is not '\n')
            {
                array[x, tmp] = chr;
                tmp++;
            }
            else
            {
                x += 1;
                tmp = y;
            }
    }

    public static string CardToString(Cartes carte)
    {
        string l1, l2, l3, l4, l5, l6;

        var elt = carte.Number switch
        {
            "Valet" => "V",
            "Dame" => "D",
            "Roi" => "R",
            "As" => "A",
            _ => carte.Number
        };

        switch (carte.Symbole)
        {
            case "Pique":
                l1 = " _____ " + "\n";
                l2 = elt.Length == 1 ? "|" + elt + " .  |" + "\n" : "|" + elt + ".  |" + "\n";
                l3 = @"| /.\ |" + "\n";
                l4 = @"|(_._)|" + "\n";
                l5 = @"|  |  |" + "\n";
                l6 = elt.Length == 1 ? "|____" + elt + "|" + "\n" : "|___" + elt + "|" + "\n";
                break;
            case "Carreau":
                l1 = " _____ " + "\n";
                l2 = elt.Length == 1 ? "|" + elt + " ^  |" + "\n" : "|" + elt + "^  |" + "\n";
                l3 = @"| / \ |" + "\n";
                l4 = @"| \ / |" + "\n";
                l5 = @"|  .  |" + "\n";
                l6 = elt.Length == 1 ? "|____" + elt + "|" + "\n" : "|___" + elt + "|" + "\n";
                break;
            case "Trefle":
                l1 = " _____ " + "\n";
                l2 = elt.Length == 1 ? "|" + elt + " _  |" + "\n" : "|" + elt + "_  |" + "\n";
                l3 = @"| ( ) |" + "\n";
                l4 = @"|(_'_)|" + "\n";
                l5 = @"|  |  |" + "\n";
                l6 = elt.Length == 1 ? "|____" + elt + "|" + "\n" : "|___" + elt + "|" + "\n";
                break;
            case "Coeur":
                l1 = " _____ " + "\n";
                l2 = elt.Length == 1 ? "|" + elt + "_ _ |" + "\n" : "|" + elt + " _ |" + "\n";
                l3 = @"|( v )|" + "\n";
                l4 = @"| \ / |" + "\n";
                l5 = @"|  .  |" + "\n";
                l6 = elt.Length == 1 ? "|____" + elt + "|" + "\n" : "|___" + elt + "|" + "\n";
                break;
            default:
                return "N/A";
        }

        var res = l1 + l2 + l3 + l4 + l5 + l6;
        return res;
    }

    public static string CartesToStringAlt(Cartes carte)
    {
        var power = carte.Power;
        var res = 0;
        while (power > 2)
        {
            res += 4;
            power--;
        }

        if (carte.Symbole == "Pique")
        {
            res += 0;
            return File.ReadAllText("cartes/" + res + ".txt");
        }

        if (carte.Symbole == "Trefle")
        {
            res += 1;
            return File.ReadAllText("cartes/" + res + ".txt");
        }

        if (carte.Symbole == "Carreau")
        {
            res += 2;
            return File.ReadAllText("cartes/" + res + ".txt");
        }

        if (carte.Symbole == "Coeur")
        {
            res += 3;
            return File.ReadAllText("cartes/" + res + ".txt");
        }

        return "N/A";
    }

    private static void ResetDisplay()
    {
        for (var i = 0; i < Height; i++)
        for (var j = 0; j < Width; j++)
            array[i, j] = '.';
    }

    private static void UpdateScreen()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        var tmp = "";
        for (var i = 0; i < Height; i++)
        for (var j = 0; j < Width; j++)
            tmp += array[i, j];

        Console.Write(tmp);
    }
}