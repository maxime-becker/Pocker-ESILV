﻿using System.Runtime.Serialization.Formatters;
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
        LaunchGame();


        //PrintGameDetails();    
    }

    #region display

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

    #endregion


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
        foreach (var player in _Players)
        {
            if (player.Argent < prix)
            {
                Console.WriteLine("Player" + player.Name + ", you don't have enought money, you cannot participate to the game");
                player.IsOut = true;
            }
            else
            {
            AutoResetPlayer:
                Console.WriteLine("Player" + player.Name + ", in order to participate to the game you need to pay " + prix);
                Console.WriteLine("Player" + player.Name + ", do you want to pay to play the game ? (y/n)");
                string choice = Console.ReadLine()!;
                if (choice == "y")
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



    private static void LaunchGame()
    {
        PrintWinner();
        PreFlop();
        Flop();
        Turn();
        River();
        PrintGameDetails();
    }

    private static void PrintWinner()
    {
        Player winner = new Player();
        int score = 0;
        foreach (var player in _Players)
        {
            if (player.IsOut == false)
            {
                if(Combinaisons.GetCombinaisons(_Table, player.Card1!, player.Card2!) > score)
                {
                    score = Combinaisons.GetCombinaisons(_Table, player.Card1!, player.Card2!);
                    winner = player;
                }
            }
        }
        System.Console.WriteLine("Winner : " + winner.Name);
        System.Console.WriteLine("Score : " + score);

    }

    private static void Turn()
    {
        PrintPartialTable(4);
        int mise = 0;
        foreach (var player in _Players)
        {
            if (player.IsOut == false)
            {
                if (player.IsCarpet is false)
                {
                    DisplayPlayerCards(player);
                    int tmp = AskForBet(player);
                    if (tmp > mise)
                    {
                        mise = tmp;
                    }
                }
                else
                {
                    System.Console.WriteLine("Player " + player.Name + " u r carpet skipping");
                }
            }
        }
        foreach (var player in _Players)
        {
            AskForAlign(player, mise);
        }
    }

    private static void River()
    {
        PrintPartialTable(5);
        int mise = 0;
        foreach (var player in _Players)
        {
            if (player.IsOut == false)
            {
                if (player.IsCarpet is false)
                {
                    DisplayPlayerCards(player);
                    int tmp = AskForBet(player);
                    if (tmp > mise)
                    {
                        mise = tmp;
                    }
                }
                else
                {
                    System.Console.WriteLine("Player " + player.Name + " u r carpet skipping");
                }
            }
        }
        foreach (var player in _Players)
        {
            AskForAlign(player, mise);
        }
    }

    private static void Flop()
    {
        PrintPartialTable(3);
        int mise = 0;
        foreach (var player in _Players)
        {
            if (player.IsOut == false)
            {
                if (player.IsCarpet is false)
                {
                    DisplayPlayerCards(player);
                    int tmp = AskForBet(player);
                    if (tmp > mise)
                    {
                        mise = tmp;
                    }
                }
                else
                {
                    System.Console.WriteLine("Player " + player.Name + " u r carpet skipping");
                }
            }
        }
        foreach (var player in _Players)
        {
            AskForAlign(player, mise);
        }
    }

    private static void PreFlop()
    {
        int mise = 0;
        foreach (var player in _Players)
        {
            DisplayPlayerCards(player);
            int tmp = AskForBet(player);
            if (tmp > mise)
            {
                mise = tmp;
            }
        }
        foreach (var player in _Players)
        {
            AskForAlign(player, mise);
        }
    }
    private static void PrintPartialTable(int index)
    {
        System.Console.WriteLine("Voici les cartes sur la table : ");
        for (int i = 0; i < index; i++)
        {
            System.Console.WriteLine(
                "   -" + i
                + ") " + _Table[i].ToString()
            );
        }
    }

    private static void AskForAlign(Player player, int mise)
    {
        if (player.IsOut || player.IsCarpet)
        {
            System.Console.WriteLine("Player " + player.Name + " u cannot align");
            return;
        }
        if (player.Argent < mise)
        {
            System.Console.WriteLine("Player " + player.Name + " do you want to follow "
           + mise + "$" + " ? (y/n)");
            System.Console.WriteLine("Your current balance is " + player.Argent);
        error1237:
            string? res = Console.ReadLine();
            if (res == "y")
            {
                player.IsCarpet = true;
            }
            if (res == "n")
            {
                player.IsOut = true;
            }
            else
            {
                System.Console.WriteLine("error, please retry");
                goto error1237;
            }


        }
        else
        {
            System.Console.WriteLine("Player " + player.Name + " do you want to follow "
            + mise + "$" + " ? (y/n)");
        AskForAlignError:
            try
            {
                string? res = Console.ReadLine();
                if (res == null)
                {
                    goto AskForAlignError;
                }
                if (res == "y")
                {
                    player.Argent -= mise;
                    MoneyStack += mise;
                    return;
                }
                if (res == "n")
                {
                    player.IsOut = true;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Input error retry");
                goto AskForAlignError;
            }
        }
    }

    private static int AskForBet(Player player)
    {
        int a = 0;
        System.Console.WriteLine("Joueur " + player.Name + " vous avez " + player.Argent + "$");
        System.Console.WriteLine("Combien voulez vous miser ?");
    AskForBetError:
        try
        {
            a = System.Convert.ToInt32(Console.ReadLine());
            if (a > player.Argent)
            {
                System.Console.WriteLine("You don't have enought money petit chenapan !");
                goto AskForBetError;

            }
        }
        catch (Exception)
        {
            System.Console.WriteLine("Erreur, veuillez réessayer");
            goto AskForBetError;
        }
        return a;

    }

    private static void DisplayPlayerCards(Player player)
    {
        System.Console.WriteLine("Joueur " + player.Name + " vous avez ces cartes en main : ");
        System.Console.WriteLine(player.Card1);
        System.Console.WriteLine(player.Card2);
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
                    false,
                    name,
                    false
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