using PockerApplication;

namespace PockerApp;

public static class Program
{
    private const int DefaultMoney = 1000;
    private static List<Cartes> _card = new();
    private static List<Player> _players = new();
    private static List<Cartes> _table = new();
    private static int _moneyStack;

    private static Cartes GetAndRemoveCard()
    {
        var number = GetRandomNumber(0, _card.Count);
        var carte = _card[number];
        _card.RemoveAt(number);
        return carte;
    }

    private static int GetRandomNumber(int min, int max)
    {
        var rnd = new Random();
        var num = rnd.Next(min, max);
        return num;
    }

    public static void Main(string[] args)
    {
        _card = Cartes.GenerateCard();
        _table = SetTable();
        _players = SetPlayers();
        GameManager.PrintTable(new List<Cartes> { _card[0] });
        Console.ReadLine();
        return;
        CreateRoom();
        LaunchGame();


        //PrintGameDetails();    
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

        foreach (var player in _players)
            if (player.Argent < prix)
            {
                Console.WriteLine("Player " + player.Name +
                                  ", you don't have enought money, you cannot participate to the game");
                player.IsOut = true;
            }
            else
            {
                AutoResetPlayer:
                Console.WriteLine("Player " + player.Name + ", in order to participate to the game you need to pay " +
                                  prix);
                Console.WriteLine("Player " + player.Name + ", do you want to pay to play the game ? (y/n)");
                var choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "y":
                        Console.WriteLine("Player " + player.Name + ", you are in the game !");
                        player.Argent -= prix;
                        _moneyStack += prix;
                        break;
                    case "n":
                        Console.WriteLine("Player " + player.Name + ", you are not in the game !");
                        break;
                    default:
                        Console.WriteLine("input error; restarting...");
                        goto AutoResetPlayer;
                }
            }
    }


    private static void LaunchGame()
    {
        PreFlop();
        Flop();
        Turn();
        River();
        PrintWinner();
    }

    private static void PrintWinner()
    {
        var winner = new Player();
        var score = 0;
        foreach (var player in _players.Where(player => player.IsOut == false).Where(player =>
                     Combinaisons.GetCombinaisons(_table, player.Card1!, player.Card2!) > score))
        {
            score = Combinaisons.GetCombinaisons(_table, player.Card1!, player.Card2!);
            winner = player;
        }

        Console.WriteLine("Winner : " + winner.Name);
        Console.WriteLine("Score : " + score);
    }

    private static void Turn()
    {
        PrintPartialTable(4);
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                DisplayPlayerCards(player);
                var tmp = AskForBet(player);
                if (tmp <= mise) continue;
                temporary = player;
                mise = tmp;
            }
            else
            {
                Console.WriteLine("Player " + player.Name + " u r carpet skipping");
            }

        foreach (var player in _players) AskForAlign(player, temporary, mise);
    }

    private static void River()
    {
        PrintPartialTable(5);
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                DisplayPlayerCards(player);
                var tmp = AskForBet(player);
                if (tmp <= mise) continue;
                temporary = player;
                mise = tmp;
            }
            else
            {
                Console.WriteLine("Player " + player.Name + " u r carpet skipping");
            }

        foreach (var player in _players) AskForAlign(player, temporary, mise);
    }

    private static void Flop()
    {
        PrintPartialTable(3);
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                DisplayPlayerCards(player);
                var tmp = AskForBet(player);
                if (tmp <= mise) continue;
                temporary = player;
                mise = tmp;
            }
            else
            {
                Console.WriteLine("Player " + player.Name + " u r carpet skipping");
            }

        foreach (var player in _players) AskForAlign(player, temporary, mise);
    }

    private static void PreFlop()
    {
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players)
        {
            DisplayPlayerCards(player);
            var tmp = AskForBet(player);
            if (tmp <= mise) continue;
            temporary = player;
            mise = tmp;
        }

        foreach (var player in _players) AskForAlign(player, temporary, mise);
    }

    private static void PrintPartialTable(int index)
    {
        Console.WriteLine("Voici les cartes sur la table : ");
        for (var i = 0; i < index; i++)
            Console.WriteLine(
                "   -" + i
                       + ") " + _table[i]
            );
    }

    private static void AskForAlign(Player player, Player better, int mise)
    {
        if (player.IsOut || player.IsCarpet)
        {
            Console.WriteLine("Player " + player.Name + " u cannot align");
            return;
        }

        if (player.Argent < mise)
        {
            Console.WriteLine("Player " + player.Name + " do you want to follow "
                              + mise + "$" + " ? (y/n)");
            Console.WriteLine("Your current balance is " + player.Argent);
            error1237:
            var res = Console.ReadLine();
            if (res == "y") player.IsCarpet = true;
            if (res == "n")
            {
                player.IsOut = true;
            }
            else
            {
                Console.WriteLine("error, please retry");
                goto error1237;
            }
        }
        else
        {
            if (player == better)
            {
                player.Argent -= mise;
                _moneyStack += mise;
                return;
            }

            Console.WriteLine("Player " + player.Name + " do you want to follow "
                              + mise + "$" + " ? (y/n)");
            AskForAlignError:
            try
            {
                var res = Console.ReadLine();
                switch (res)
                {
                    case null:
                        goto AskForAlignError;
                    case "y":
                        player.Argent -= mise;
                        _moneyStack += mise;
                        return;
                    case "n":
                        player.IsOut = true;
                        break;
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
        int a;
        Console.WriteLine("Joueur " + player.Name + " vous avez " + player.Argent + "$");
        Console.WriteLine("Combien voulez vous miser ?");
        AskForBetError:
        try
        {
            a = Convert.ToInt32(Console.ReadLine());
            if (a > player.Argent)
            {
                Console.WriteLine("You don't have enought money petit chenapan !");
                goto AskForBetError;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Erreur, veuillez réessayer");
            goto AskForBetError;
        }

        return a;
    }

    private static void DisplayPlayerCards(Player player)
    {
        Console.WriteLine("Joueur " + player.Name + " vous avez ces cartes en main : ");
        Console.WriteLine(player.Card1);
        Console.WriteLine(player.Card2);
    }

    private static List<Player> SetPlayers()
    {
        int nbPlayers;
        var playsersList = new List<Player>();
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

        for (var i = 0; i < nbPlayers; i++)
        {
            Console.WriteLine("Input Name for player {0}", i);
            var name = Console.ReadLine()!;
            var player = new Player
            (
                GetAndRemoveCard(),
                GetAndRemoveCard(),
                DefaultMoney,
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
        var table = new List<Cartes>();
        for (var i = 0; i < 5; i++) table.Add(GetAndRemoveCard());
        return table;
    }

    #region display

    private static void PrintGameDetails()
    {
        PrintCards();
        PrintPlayers();
        Console.WriteLine("Default Money : " + DefaultMoney + "Money stack : " + _moneyStack);
        PrintTable();
    }

    private static void PrintTable()
    {
        Console.WriteLine("******************TABLE*****************");

        foreach (var card in _table) Console.WriteLine(card.ToString());
        Console.WriteLine("******************ENDTABLE*****************");
    }

    private static void PrintCards()
    {
        Console.WriteLine("******************CARDS*****************");

        foreach (var card in _card) Console.WriteLine(card.ToString());
        Console.WriteLine("******************END*****************");
    }

    private static void PrintPlayers()
    {
        Console.WriteLine("******************PLAYERS*****************");

        foreach (var player in _players) Console.WriteLine(player.ToString());
        Console.WriteLine("******************ENDPLAYERS*****************");
    }

    #endregion
}