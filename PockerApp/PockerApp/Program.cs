namespace PockerApp;

public static class Program
{
    private const int DefaultMoney = 1000;
    private static List<Cartes> _card = new();
    private static readonly List<Player> _players = new();
    private static List<Cartes> _table = new();
    private static int _moneyStack;
    private static Logs _logs;

    private static Cartes GetAndRemoveCard() // retire une carte du paquet et la retourne
    {
        var number = GetRandomNumber(0, _card.Count);
        var carte = _card[number];
        _card.RemoveAt(number);
        return carte;
    }

    private static int GetRandomNumber(int min, int max) // Donne un nombre entre min et max aléatoirement
    {
        var rnd = new Random();
        var num = rnd.Next(min, max);
        return num;
    }

    public static void Main(string[] args)
    {
        _logs = new Logs("Launching game...");
        Console.Clear();
        GameManager.InitDisplay();
        _card = Cartes.GenerateCard();
        _table = SetTable();
        CreateOrLoadPlayers();
        CreateRoom();
        LaunchGame();

        Console.ReadLine();
        //PrintGameDetails();    
    }


    private static void CreateRoom()
    {
        _logs.LogWrite("Création d'une salle de jeux en cours");
        int prix;
        AutoResetEvent:
        Console.WriteLine("Merci d'entrer le prix d'entrée de la partie : ");
        try
        {
            prix = Convert.ToInt32(Console.ReadLine());
            _logs.LogWrite("Le prix de la partie est : " + prix);
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
                _logs.LogWrite("Joueur " + player.Name +
                               ", ne peut pas participer");

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
                        _logs.LogWrite("Joueur " + player.Name +
                                       ", joue !");
                        player.Argent -= prix;
                        _moneyStack += prix;
                        break;
                    case "n":
                        Console.WriteLine("Joueur " + player.Name + ", vous n'êtes pas dans la partie !");
                        _logs.LogWrite("Joueur " + player.Name +
                                       ", ne joue pas !");
                        break;
                    default:
                        Console.WriteLine("erreur");
                        goto AutoResetPlayer;
                }
            }

        _logs.LogWrite("Création d'une salle de jeux terminée");
    }

    private static void LaunchGame()
    {
        if (CheckIfAlone()) PreFlop();
        if (CheckIfAlone()) Flop();
        if (CheckIfAlone()) Turn();
        if (CheckIfAlone()) River();
        PrintWinner();
        UpdateData();
    }

    public static bool CheckIfAlone() // Vérifie si il reste un joueur au moins
    {
        var cnt = 0;
        foreach (var player in _players)
            if (!player.IsOut)
                cnt++;
        if (cnt == 1)
            return false;
        return true;
    }

    public static void UpdateData() // Fin de partie dans les fichiers 
    {
        _logs.LogWrite(@"Mise à jour des données des joueurs");
        foreach (var players in _players)
            File.WriteAllText(@"players\" + players.Name + ".csv", players.Argent.ToString());
        _logs.LogWrite(@"Mise à jour des données des joueurs faite");

        if (SafeBoolUserInput("Voulez vous relancer une partie ?")) Main(null);
    }

    private static void PrintWinner() // Affichage du winner 
    {
        var winner = new Player();
        var score = 0;
        foreach (var player in _players.Where(player => player.IsOut == false).Where(
                     player => //LINQ optimisiation proposée par visual studio je cherche parmi les joueurs 
                         Combinaisons.GetCombinaisons(_table, player.Card1!, player.Card2!) >
                         score)) // En jeu, celui qui a le plus haut score
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
                Console.WriteLine("Player " + player.Name + " tapis");
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
                Console.WriteLine("Player " + player.Name + " tapis");
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
                Console.WriteLine("Player " + player.Name + " tapis");
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

    private static void
        AskForAlign(Player player, Player better, int mise) // Demande aux joueur de s'aligner sur la mise la plus haute
    {
        Console.Clear();
        if (player.IsOut || player.IsCarpet)
        {
            Console.WriteLine("Player " + player.Name + " vous ne pouvez pas vous alligner");
            return;
        }

        if (player.Argent < mise)
        {
            Console.WriteLine("Player " + player.Name + " voulez vous suivre la plus grosse mise ? "
                              + mise + "$" + " ? (y/n)");
            Console.WriteLine("Argent sur le compte : " + player.Argent);
            error1237:
            var res = Console.ReadLine();
            switch (res)
            {
                case "y":
                    player.IsCarpet = true;
                    player.Argent = 0;
                    break;
                case "n":
                    player.IsOut = true;
                    break;
                default:
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

            Console.WriteLine("Joueur " + player.Name + " voulez vous suivre la mise "
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

    public static void Transition(string name) // petite transition entre joueurs 
    {
        Console.WriteLine("C'est le tour de " + name);
        Thread.Sleep(3000);
        Console.Clear();
    }

    private static int AskForBet(Player player) // Demande aux joueurs de parier 
    {
        Transition(player.Name!);
        int a;
        Console.WriteLine("Joueur " + player.Name + " vous avez " + player.Argent + "$");
        Console.WriteLine("Combien voulez vous miser ?");
        Jesaisquejépaldroitmaissivouplé:
        try
        {
            a = Convert.ToInt32(Console.ReadLine());


            if (a > player.Argent)
            {
                Console.WriteLine("You don't have enought money petit chenapan !");
                goto Jesaisquejépaldroitmaissivouplé;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Erreur, veuillez réessayer");
            goto Jesaisquejépaldroitmaissivouplé;
        }

        return a;
    }

    private static void CreateOrLoadPlayers()
    {
        while (true)
        {
            if (!Directory.Exists("players")) Directory.CreateDirectory("players");

            ReadPlayers();
            while (SafeBoolUserInput("Voulez vous creer des nouveaux joueurs ? (y/n)")) CreateAndSavePlayer();

            if (_players.Count < 2)
            {
                Console.WriteLine("Il manque des joueurs");
                continue;
            }

            break;
        }
    }

    public static void ReadPlayers()
    {
        Console.WriteLine("Il y a " + Directory.GetFiles("players/").Length + " comptes enregistrés");
        foreach (var variable in Directory.GetFiles("players/"))
        {
            Console.WriteLine("Profile " + Path.GetFileName(variable) + " found");
            Console.WriteLine("Voulez vous que le joueur " + Path.GetFileName(variable) + " joue ?");
            if (SafeBoolUserInput(""))
            {
                var player = new Player
                (
                    GetAndRemoveCard(),
                    GetAndRemoveCard(),
                    Convert.ToInt32(File.ReadAllText(variable)),
                    false,
                    Path.GetFileName(variable)[..(Path.GetFileName(variable).Length - 4)],
                    false
                );
                _players.Add(player);
            }
        }
    }

    private static void CreateAndSavePlayer()
    {
        _logs.LogWrite(@"Creation joueur en cours");

        Console.WriteLine("Nom du joueur :");
        var name = SafeStringUserInput();
        Console.WriteLine("Argent du joueur : ");
        var money = SafeIntUserInput();
        File.WriteAllText("players/" + name + ".csv", money.ToString());
        var player = new Player
        (
            GetAndRemoveCard(),
            GetAndRemoveCard(),
            Convert.ToInt32(File.ReadAllText("players/" + name + ".csv")),
            false,
            Path.GetFileName(Path.GetFileName("players/" + name + ".csv")),
            false
        );
        _players.Add(player);
        _logs.LogWrite(@"creation joueurs faite !");
    }

    private static bool SafeBoolUserInput(string s)
    {
        Console.WriteLine(s);
        try
        {
            var res = Console.ReadLine();
            switch (res)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("Entrée non valide");
                    return SafeBoolUserInput(s);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Entrée non valide");
            return SafeBoolUserInput(s);
        }
    }

    private static string SafeStringUserInput()
    {
        try
        {
            var res = Console.ReadLine();
            if (res is null) throw new Exception();
            return res;
        }
        catch (Exception)
        {
            Console.WriteLine("Entrée non valide");
            return SafeStringUserInput();
        }
    }

    private static int SafeIntUserInput()
    {
        try
        {
            var res = Convert.ToInt32(Console.ReadLine());
            return res;
        }
        catch (Exception)
        {
            Console.WriteLine("Entrée non valide");
            return SafeIntUserInput();
        }
    }

    #region set

    private static List<Cartes> SetTable()
    {
        var table = new List<Cartes>();
        for (var i = 0; i < 5; i++) table.Add(GetAndRemoveCard());
        return table;
    }

    #endregion

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