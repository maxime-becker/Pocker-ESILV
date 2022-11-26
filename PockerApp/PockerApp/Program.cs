using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;

namespace PockerApp;

public class Program
{
    public static void Main(string[] args)
    {
        List<Cartes> maList = Cartes.GenerateCard();
        for (int i = 0; i < maList.Count; i++)
        {
            Console.WriteLine(i + ")" + maList[i].ToString());
        }

        List<Cartes> maList2 = new List<Cartes> 
        { 
            maList[0],
            maList[1],
            maList[2],
            maList[43],
            maList[12]
        
        };

        Cartes carte1 = maList[37];
        Cartes carte2 = maList[44];

        Combinaisons player1 = new Combinaisons(maList2, carte1, carte2);
        Console.WriteLine(player1.Combinaison);
    }
}