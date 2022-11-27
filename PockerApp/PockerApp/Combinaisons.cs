using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        #region StaticFunctions

        public static int GetMaxPower(List<Cartes> list)
        {
            int max = list[0].Power;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].Power > max)
                {
                    max = list[i].Power;
                }
            }
            return max;
        }

        public static void SortByPower(List<Cartes> cartes)
        {
            for (int i = 0; i < cartes.Count; i++)
            {
                for (int j = i + 1; j < cartes.Count; j++)
                {
                    if (cartes[j].Power < cartes[i].Power)
                    {
                        var tmp = cartes[j];
                        cartes[j] = cartes[i];
                        cartes[i] = tmp;
                    }
                }
            }
        }

        #endregion

        public Combinaisons(List<Cartes> tapis, Cartes carte1, Cartes carte2)
        {
            Combinaison = 0;

            List<Cartes> list = new List<Cartes>();
            foreach (Cartes carte in tapis)
            {
                list.Add(carte);
            }
            list.Add(carte1);
            list.Add(carte2);

            if (isQuinteFlushRoyale(list))
            {
                Combinaison += 1000;
                return;
            }
            if (isQuinteFlush(list))
            {
                Combinaison += 900;
                return;
            }
            if (isSquare(list))
            {
                Combinaison += 800;
                return;
            }
            if (isFull(list))
            {
                Combinaison += 700;
                return;
            }
            if (isColor(list))
            {
                Combinaison += 600;
                return;
            }
            if (isQuinte(list))
            {
                Combinaison += 500;
                return;
            }
            if(isBrelan(list))
            {
                Combinaison += 400;
                return;
            }
            if(isTwoPairs(list))
            {
                Combinaison += 300;
                return;
            }
            if(isPair(list))
            {
                Combinaison += 200;
                return;
            }

            else
            {
                Combinaison += isDefault(list);
                return;
            }
        }

        public bool isQuinteFlushRoyale(List<Cartes> cartes)
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
            if (rouged.Count > 4)
            {
                int suite = 0;
                for (int i = 0; i < rouged.Count - 1; i++)
                {
                    if (suite == 4)
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
                    if (suite == 4)
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
                if (count == 4)
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
            SortByPower(cartes);
            int val = 0;
            Cartes tmp = cartes[0];
            bool isThree = false;
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power == cartes[i + 1].Power)
                {
                    val++;
                }
                else
                {
                    val = 0;
                }
                if(val == 2)
                {
                    tmp = cartes[i];
                    isThree = true;
                    break;
                }
            }
            if(isThree is false)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < cartes.Count - 1; i++)
                {
                    if (cartes[i].Power == cartes[i + 1].Power && cartes[i].Power != tmp.Power)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isColor(List<Cartes> cartes)
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
            return blacked.Count >= 5 ? true : rouged.Count >= 5 ? true : false;
            //if (blacked.Count > 5) 
            //{
            //    return true;
            //}
            //else
            //{
            //    if (rouged.Count > 5)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //il regarde si il y a 5 cartes noirs (si oui il renvoit true, sinon il regarde si il y a 5 >= cartes rouges si oui true sinon false. 
        }

        public bool isQuinte(List<Cartes> cartes)
        {
            SortByPower(cartes);
            int suite = 0;
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power - cartes[i + 1].Power == -1)
                {
                    suite++;
                }
                else
                {
                    suite = 0;
                }
                if (suite == 4)
                {
                    return true;
                }
            }
            foreach(var card in cartes)
            {
                if(card.Power == 14)
                {
                    card.Power = 1;
                }
            }
            SortByPower(cartes);
            suite = 0;
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power - cartes[i + 1].Power == -1)
                {
                    suite++;
                }
                else
                {
                    suite = 0;
                }
                if (suite == 4)
                {
                    foreach (var card in cartes)
                    {
                        if (card.Power == 1)
                        {
                            card.Power = 14;
                        }
                    }
                    return true;
                }
            }
            foreach (var card in cartes)
            {
                if (card.Power == 1)
                {
                    card.Power = 14;
                }
            }
            return false;
        }

        public bool isBrelan(List<Cartes> cartes)
        {
            SortByPower(cartes);
            int same = 0;
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power == cartes[i+1].Power)
                {
                    same++;
                }
                else
                {
                    same = 0;
                }
                if(same == 2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isTwoPairs(List<Cartes> cartes)
        {
            SortByPower(cartes);
            int nbPair = 0;
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power == cartes[i+1].Power)
                {
                    nbPair += 1;
                }
                if(nbPair == 2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPair(List<Cartes> cartes)
        {
            SortByPower(cartes);
            for (int i = 0; i < cartes.Count - 1; i++)
            {
                if (cartes[i].Power == cartes[i + 1].Power)
                {
                    return true;
                }
            }
            return false;
        }

        public int isDefault(List<Cartes> cartes)
        {
            SortByPower(cartes);
            return cartes[cartes.Count - 1].Power;
        }
    }
}
