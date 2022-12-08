namespace PockerApp;

public class Cartes
{
    public Cartes(int number, string symbole, int power) //création de ma carte 
    {
        Number = number switch
        {
            11 => "Valet",
            12 => "Dame",
            13 => "Roi",
            14 => "As",
            _ => number.ToString()
        };

        Symbole = symbole;
        Power = power;
    }

    public int Power { get; set; }
    public string Number { get; set; }
    public string Symbole { get; set; }

    public override string ToString()
    {
        return "Symbole : " + Symbole + " /  Numero : " + Number + "  /  Power : " + Power;
    }

    public static void WriteCards(List<Cartes> cartesList)
    {
        if (!Directory.Exists("cartes"))
            Directory.CreateDirectory("cartes");
        var i = 0;
        foreach (var carte in cartesList)
        {
            File.WriteAllText(@"cartes\" + i + ".txt", GameManager.CardToString(carte));
            i++;
        }
    }

    public static List<Cartes> GenerateCard()
    {
        var cards = new List<Cartes>();
        for (var i = 2; i < 15; i++)
        {
            /*
            Cartes Pique = new Cartes(i, "Pique");
            Cartes Trefle = new Cartes(i, "Trefle");
            Cartes Carreau = new Cartes(i, "Carreau");
            Cartes Coeur = new Cartes(i, "Coeur"); */ //je fais la meme chose juste en bas

            cards.Add(new Cartes(i, "Pique", i));
            cards.Add(new Cartes(i, "Trefle", i));
            cards.Add(new Cartes(i, "Carreau", i));
            cards.Add(new Cartes(i, "Coeur", i));
        }

        return cards;
    }
}