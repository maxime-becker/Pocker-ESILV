namespace PockerApp;

internal class Player
{
    public Player(Cartes card1, Cartes card2, int argent, bool isOut, string name, bool isCarpet)
    {
        Card1 = card1;
        Card2 = card2;
        Argent = argent;
        IsOut = isOut;
        Name = name;
        IsCarpet = isCarpet;
    }

    public Player()
    {
    }

    public Cartes? Card1 { get; set; }
    public Cartes? Card2 { get; set; }
    public int Argent { get; set; }
    public bool IsOut { get; set; }
    public string? Name { get; set; }
    public bool IsCarpet { get; set; }

    public override string? ToString()
    {
        return "Carte 1 : " + Card1 + " / Carte 2 : " + Card2 + "  /  Argent : " + Argent + "  /  Is out ? : " + IsOut +
               "  /  Nom : " + Name + " / Is Carpet ? : " + IsCarpet;
    }
}