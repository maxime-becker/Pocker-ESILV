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
        GameManager.InitDisplay();
        _card = Cartes.GenerateCard();
        Cartes.WriteCards(_card);
        _table = SetTable();
        _players = SetPlayers();
        CreateRoom();
        LaunchGame();

        Console.ReadLine();
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
                Console.WriteLine("Joueur " + player.Name +
                                  ", vous n'avez pas assez d'argent pour participer au jeu");
                player.IsOut = true;
            }
            else
            {
                AutoResetPlayer:
                Console.WriteLine("Joueur " + player.Name + ", pour jouer vous devez payer "
                                  + prix + "$");
                Console.WriteLine("Joueur " + player.Name + ", voulez vous payer pour rejoindre la partie ? (y/n)");
                var choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "y":
                        Console.WriteLine("Joueur " + player.Name + ", vous êtes dans la partie !");
                        player.Argent -= prix;
                        _moneyStack += prix;
                        break;
                    case "n":
                        Console.WriteLine("Joueur " + player.Name + ", vous n'êtes pas dans la partie !");
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

        winner.Argent += _moneyStack;
        _moneyStack = 0;
        Console.WriteLine("Ths Winner is " + winner.Name);
        Console.WriteLine("with score : " + score);
        foreach (var player in _players)
        {
            player.IsOut = false;
            player.IsCarpet = false;
        }
    }

    private static void Turn()
    {
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                GameManager.PrintRound(_table, 4, player);
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
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                GameManager.PrintRound(_table, 5, player);
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
        GameManager.PrintTable(_table, 3);
        var mise = 0;
        Player temporary = null!;
        foreach (var player in _players.Where(player => player.IsOut == false))
            if (player.IsCarpet is false)
            {
                GameManager.PrintRound(_table, 3, player);
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
            GameManager.PrintRound(_table, 0, player);
            var tmp = AskForBet(player);
            if (tmp <= mise) continue;
            temporary = player;
            mise = tmp;
        }

        foreach (var player in _players) AskForAlign(player, temporary, mise);
    }

    private static void AskForAlign(Player player, Player better, int mise)
    {
        Console.Clear();
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
            Console.WriteLine("Merci de taper le nom du joueur " + i);
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