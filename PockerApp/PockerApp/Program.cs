using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;

namespace PockerApp;

public static class Program
{
    internal static List<Cartes> _Cartes = new List<Cartes>();
    internal static List<Player> _Players = new List<Player>();
    internal static List<Cartes> _Table = new List<Cartes>();

    internal static int DefaultMoney = 1000;
    internal static int MoneyStack = 0;

    public static Cartes GetAndRemoveCard()
    {
        int number = GetRandomNumber(0, _Cartes.Count);
        Cartes carte = _Cartes[number];
        _Cartes.RemoveAt(number);
        return carte;
    }

    public static int GetRandomNumber(int min, int max)
    {
        Random rnd = new Random();
        int num = rnd.Next(min, max);
        return num;
    }

    public static void Main(string[] args)
    {
        _Cartes = Cartes.GenerateCard();
        _Table = SetTable();
        _Players = SetPlayers();


        CreateRoom();
        PrintGameDetails();

    }

    public static void PrintGameDetails()
    {
        PrintCards();
        PrintPlayers();
        Console.WriteLine("Default Money : " + DefaultMoney + "Money stack : " + MoneyStack);
        PrintTable();
    }

    public static void PrintTable()
    {
        Console.WriteLine("******************TABLE*****************");

        foreach (var card in _Table)
        {
            Console.WriteLine(card.ToString());
        }
        Console.WriteLine("******************ENDTABLE*****************");
    }

    public static void PrintCards()
    {
        Console.WriteLine("******************CARDS*****************");

        foreach (var card in _Cartes)
        {
            Console.WriteLine(card.ToString());
        }
        Console.WriteLine("******************END*****************");

    }

    public static void PrintPlayers()
    {
        Console.WriteLine("******************PLAYERS*****************");

        foreach (var player in _Players)
        {
            Console.WriteLine(player.ToString());

        }
        Console.WriteLine("******************ENDPLAYERS*****************");

    }

    private static void CreateRoom()
    {
        int prix;
        AutoResetEvent:
        Console.WriteLine("Merci d'entrer le prix d'entrée de la partie : ");
        try
        {
            prix = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Input error");
            goto AutoResetEvent;
        }
        foreach(var player in _Players)
        {
            if (player.Argent < prix)
            {
                Console.WriteLine("Player" + player.Name + ", you don't have enought money, you cannot participate to the game");
                player.CanPlay = false;
            }
            else
            {
                AutoResetPlayer:
                Console.WriteLine("Player" + player.Name + ", in order to participate to the game you need to pay " + prix);
                Console.WriteLine("Player" + player.Name + ", do you want to pay to play the game ? (y/n)");
                string choice = Console.ReadLine()!;
                if ( choice == "y")
                {
                    Console.WriteLine("Player " + player.Name + ", you are in the game !");
                    player.Argent -= prix;
                    MoneyStack += prix;
                }
                else
                {
                    if (choice == "n")
                    {
                        Console.WriteLine("Player " + player.Name + ", you are not in the game !");
                    }
                    else
                    {
                        Console.WriteLine("input error; restarting...");
                        goto AutoResetPlayer;
                    }
                }
            }
        }
    }

    private static List<Player> SetPlayers()
    {
        int nbPlayers;
        List<Player> playsersList = new List<Player>();
        Console.WriteLine("How many players will play ? ");
    AutoResetEvent:
        try
        {
            nbPlayers = Convert.ToInt32(Console.ReadLine());
            if (nbPlayers < 2)
            {
                Console.WriteLine("Program needs 2+ players");
                goto AutoResetEvent;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Input error");
            goto AutoResetEvent;
        }
        for (int i = 0; i < nbPlayers; i++)
        {
            Console.WriteLine("Input Name for player {0}", i);
            string name = Console.ReadLine()!;
            Player player = new Player
                (
                    Program.GetAndRemoveCard(),
                    Program.GetAndRemoveCard(),
                    Program.DefaultMoney,
                    true,
                    name
                );
            playsersList.Add(player);
        }
        return playsersList;
    }

    private static List<Cartes> SetTable()
    {
        List<Cartes> table = new List<Cartes>();
        for (int i = 0; i < 5; i++)
        {
            table.Add(GetAndRemoveCard());
        }
        return table;
    }

}