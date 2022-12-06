namespace PockerApp;

internal class GameManager
{
    private static int Width;
    private static int Height;
    private static char[,] array;

    private static void InitDisplay()
    {
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
        array = new char[Height, Width];
    }

    public static void PrintTable(List<Cartes> cardList)
    {
        InitDisplay();
        ResetDisplay();
        foreach (var card in cardList) AddTextToArray(Height / 2 - 3, Width / 2 - 3, CardToString(card));

        UpdateScreen();
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

    private static string CardToString(Cartes carte)
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