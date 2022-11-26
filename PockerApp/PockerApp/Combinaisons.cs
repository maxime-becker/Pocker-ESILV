using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace PockerApp
{
    internal class Combinaisons
    {
        public int Combinaison { get; }

        public Combinaisons(List<Cartes> tapis, Cartes carte1, Cartes carte2)
        {
            List<Cartes> list = new List<Cartes>();
            foreach (Cartes carte in tapis)
            {
                list.Add(carte);
            }
            list.Add(carte1);
            list.Add(carte2);

            if (isQuinteFlushRoyale(list))
            {
                Combinaison = 1000;
                return;
            }
            if(isQuinteFlush(list))
            {
                Combinaison = 999;
                return;
            }
            if(isSquare(list))
            {
                Combinaison = 998;
                return;
            }
            if(isFull(list))
            {
                Combinaison= 997;
                return;
            }
            Combinaison = -1;
        }

        public bool isQuinteFlushRoyale(List<Cartes> cartes)
        {
            List<Cartes> blacked = new List<Cartes>();
            List<Cartes> rouged = new List<Cartes>();

            foreach(var card in cartes)
            {
                if(card.Symbole is "Pique" or "Trefle")
                {
                    blacked.Add(card);
                }
                if(card.Symbole is "Coeur" or "Carreau")
                {
                    rouged.Add(card);
                }
            }
            if (rouged.Count >= 5)
            {
                bool have10 = false,
                            have11 = false,
                            have12 = false,
                            have13 = false,
                            have14 = false;

                foreach (var card in rouged)
                {
                    if (card.Power == 10)
                    {
                        have10 = true;
                    }
                    if (card.Power == 11)
                    {
                        have11 = true;
                    }
                    if (card.Power == 12)
                    {
                        have12 = true;
                    }
                    if (card.Power == 13)
                    {
                        have13 = true;
                    }
                    if (card.Power == 14)
                    {
                        have14 = true;
                    }
                }
                if (have10 && have11 && have12 && have13 && have14)
                {
                    return true;
                }
            }
            if (blacked.Count >= 5)
            {
                bool have10 = false,
                            have11 = false,
                            have12 = false,
                            have13 = false,
                            have14 = false;

                foreach (var card in blacked)
                {
                    if (card.Power == 10)
                    {
                        have10 = true;
                    }
                    if (card.Power == 11)
                    {
                        have11 = true;
                    }
                    if (card.Power == 12)
                    {
                        have12 = true;
                    }
                    if (card.Power == 13)
                    {
                        have13 = true;
                    }
                    if (card.Power == 14)
                    {
                        have14 = true;
                    }
                }
                if (have10 && have11 && have12 && have13 && have14)
                {
                    return true;
                }
            }


            return false;

        }

        public bool isQuinteFlush(List<Cartes> cartes)
        {
            List<Cartes> blacked = new List<Cartes>();
            List<Cartes> rouged = new List<Cartes>();

            foreach (var card in cartes)
            {
                if (card.Symbole is "Pique" or "Trefle")
                {
                    blacked.Add(card);
                }
                if (card.Symbole is "Coeur" or "Carreau")
                {
                    rouged.Add(card);
                }
            }
            for (int i = 0; i < blacked.Count; i++)
            {
                for (int j = i + 1; j < blacked.Count; j++)
                {
                    if (blacked[j].Power < blacked[i].Power)
                    {
                        var tmp = blacked[j];
                        blacked[j] = blacked[i];
                        blacked[i] = tmp;
                    }
                }
            }
            for (int i = 0; i < rouged.Count; i++)
            {
                for (int j = i + 1; j < rouged.Count; j++)
                {
                    if (rouged[j].Power < rouged[i].Power)
                    {
                        var tmp = rouged[j];
                        rouged[j] = rouged[i];
                        rouged[i] = tmp;
                    }
                }
            }
            if(rouged.Count > 4) 
            {
                int suite = 0;
                for (int i = 0; i < rouged.Count - 1; i++)
                {
                    if(suite == 5)
                    {
                        return true; 
                    }
                    if (rouged[i].Power - rouged[i + 1].Power == -1)
                    {
                        suite++;
                    }
                    else
                    {
                        suite = 0;
                    }
                }
            }
            if (blacked.Count > 4)
            {
                int suite = 0;
                for (int i = 0; i < blacked.Count - 1; i++)
                {
                    if (blacked[i].Power - blacked[i + 1].Power == -1)
                    {
                        suite++;
                    }
                    else
                    {
                        suite = 0;
                    }
                    if (suite == 5)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isSquare(List<Cartes> cartes)
        {
            for (int i = 0; i < cartes.Count; i++)
            {
                int count = 1;
                for (int j = i + 1; j < cartes.Count; j++)
                {
                    if (cartes[j].Number == cartes[i].Number)
                    {
                        count++;
                    }
                }
                if(count == 4)
                { 
                    return true; 
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        public bool isFull(List<Cartes> cartes)
        {
            string three = "";
            for (int i = 0; i < cartes.Count; i++)
            {
                int count = 1;
                for (int j = i + 1; j < cartes.Count; j++)
                {
                    if (cartes[j].Number == cartes[i].Number)
                    {
                        three = cartes[i].Number;
                        count++;
                    }
                }
                if (count == 3)
                {
                    for (int k = 0; k < cartes.Count; k++)
                    {
                        int count2 = 1;
                        for (int l = k + 1; l < cartes.Count; l++)
                        {
                            if (cartes[k].Number == cartes[l].Number && cartes[l].Number != three)
                            {
                                count2++;
                            }
                        }
                        if(count2 == 2)
                        {
                            return true;
                        }                       
                    }
                }              
            }
            return false;
        }
    }
}
