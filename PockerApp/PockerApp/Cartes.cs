using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PockerApp
{
    internal class Cartes
    {
        public int Power { get; set; }
        public string Number { get; set; }
        public string Symbole { get; set; }
        public Cartes(int number,string symbole, int power) //création de ma carte 
        {
            switch (number)
            {
                case 11:
                    Number = "Valet";
                    break;
                case 12:
                    Number = "Dame";
                    break;
                case 13:
                    Number = "Roi";
                    break;
                case 14:
                    Number = "As";
                    break;
                
                default:
                    Number = number.ToString();
                    break;
            }
            Symbole = symbole;
            Power = power; 
        }
        public override string? ToString()
        {
            return "Symbole : "+Symbole+" /  Numero : "+Number+"  /  Power : "+Power;
        }
        public static List<Cartes> GenerateCard()
        {
            List<Cartes> cards = new List<Cartes>();
            for (int i = 2; i < 15; i++)
            {
                /*
                Cartes Pique = new Cartes(i, "Pique");
                Cartes Trefle = new Cartes(i, "Trefle");
                Cartes Carreau = new Cartes(i, "Carreau");
                Cartes Coeur = new Cartes(i, "Coeur"); */  //je fais la meme chose juste en bas

                cards.Add(new Cartes(i,"Pique",i));
                cards.Add(new Cartes(i,"Trefle",i));
                cards.Add(new Cartes(i,"Carreau",i));
                cards.Add(new Cartes(i,"Coeur",i));

            }
            return cards;
        }
    }
}
