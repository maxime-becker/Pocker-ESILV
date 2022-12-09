namespace PockerApp;

//L'objectif de cette classe est qu'a partir des 5 cartes de la table et des 2 cartes en main d'un joueur je sois en
//mesure de définir un score correpondant à la plus haute combinaison. 
//Chaque fonction a un nom explicite 

internal static class Combinaisons
{
    private static int _combinaison;

    public static int GetCombinaisons(List<Cartes> tapis, Cartes carte1, Cartes carte2)
    {
        _combinaison = 0;

        var list = tapis.ToList();
        list.Add(carte1);
        list.Add(carte2);

        if (isQuinteFlushRoyale(list))
        {
            _combinaison += 1000;
            return _combinaison;
        }

        if (isQuinteFlush(list))
        {
            _combinaison += 900;
            return _combinaison;
        }

        if (isSquare(list))
        {
            _combinaison += 800;
            return _combinaison;
        }

        if (isFull(list))
        {
            _combinaison += 700;
            return _combinaison;
        }

        if (isColor(list))
        {
            _combinaison += 600;
            return _combinaison;
        }

        if (isQuinte(list))
        {
            _combinaison += 500;
            return _combinaison;
        }

        if (isBrelan(list))
        {
            _combinaison += 400;
            return _combinaison;
        }

        if (isTwoPairs(list))
        {
            _combinaison += 300;
            return _combinaison;
        }

        if (isPair(list))
        {
            _combinaison += 200;
            return _combinaison;
        }

        _combinaison += isDefault(list);
        return _combinaison;
    }

    private static bool isQuinteFlushRoyale(List<Cartes> cartes)
    {
        var blacked = new List<Cartes>();
        var rouged = new List<Cartes>();

        foreach (var card in cartes)
        {
            switch (card.Symbole)
            {
                case "Pique" or "Trefle":
                    blacked.Add(card);
                    break;
                case "Coeur" or "Carreau":
                    rouged.Add(card);
                    break;
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
                if (card.Power == 10) have10 = true;
                if (card.Power == 11) have11 = true;
                if (card.Power == 12) have12 = true;
                if (card.Power == 13) have13 = true;
                if (card.Power == 14) have14 = true;
            }

            if (have10 && have11 && have12 && have13 && have14) return true;
        }

        if (blacked.Count < 5) return false;
        {
            bool have10 = false,
                have11 = false,
                have12 = false,
                have13 = false,
                have14 = false;

            foreach (var card in blacked)
            {
                if (card.Power == 10) have10 = true;
                if (card.Power == 11) have11 = true;
                if (card.Power == 12) have12 = true;
                if (card.Power == 13) have13 = true;
                if (card.Power == 14) have14 = true;
            }

            if (have10 && have11 && have12 && have13 && have14) return true;
        }


        return false;
    }

    private static bool isQuinteFlush(List<Cartes> cartes)
    {
        var blacked = new List<Cartes>();
        var rouged = new List<Cartes>();

        foreach (var card in cartes)
        {
            switch (card.Symbole)
            {
                case "Pique" or "Trefle":
                    blacked.Add(card);
                    break;
                case "Coeur" or "Carreau":
                    rouged.Add(card);
                    break;
            }
        }

        for (var i = 0; i < blacked.Count; i++)
        for (var j = i + 1; j < blacked.Count; j++)
            if (blacked[j].Power < blacked[i].Power)
            {
                (blacked[j], blacked[i]) = (blacked[i], blacked[j]);
            }

        for (var i = 0; i < rouged.Count; i++)
        for (var j = i + 1; j < rouged.Count; j++)
            if (rouged[j].Power < rouged[i].Power)
            {
                (rouged[j], rouged[i]) = (rouged[i], rouged[j]);
            }

        if (rouged.Count > 4)
        {
            var suite = 0;
            for (var i = 0; i < rouged.Count - 1; i++)
            {
                if (suite == 4)
                {
                    _combinaison += rouged[i].Power;
                    return true;
                }

                if (rouged[i].Power - rouged[i + 1].Power == -1)
                    suite++;
                else
                    suite = 0;
            }
        }

        if (blacked.Count <= 4) return false;
        {
            var suite = 0;
            for (var i = 0; i < blacked.Count - 1; i++)
            {
                if (suite == 4)
                {
                    _combinaison += blacked[i].Power;
                    return true;
                }

                if (blacked[i].Power - blacked[i + 1].Power == -1)
                    suite++;
                else
                    suite = 0;
            }
        }

        return false;
    }

    private static bool isSquare(List<Cartes> cartes)
    {
        SortByPower(cartes);
        var tmp = 0;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (tmp == 4)
            {
                _combinaison += cartes[i].Power;
                return true;
            }

            if (cartes[i].Power == cartes[i + 1].Power)
            {
                tmp += 1;
            }
            else
            {
                tmp = 0;
            }
        }

        return false;
    }

    private static bool isFull(List<Cartes> cartes)
    {
        SortByPower(cartes);
        var val = 0;
        var tmp = cartes[0];
        var isThree = false;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (cartes[i].Power == cartes[i + 1].Power)
                val++;
            else
                val = 0;
            if (val != 2) continue;
            tmp = cartes[i];
            _combinaison += tmp.Power;
            isThree = true;
            break;
        }

        if (isThree is false) return false;

        for (var i = 0; i < cartes.Count - 1; i++)
            if (cartes[i].Power == cartes[i + 1].Power && cartes[i].Power != tmp.Power)
                return true;
        return false;
    }

    private static bool isColor(List<Cartes> cartes)
    {
        var blacked = new List<Cartes>();
        var rouged = new List<Cartes>();
        foreach (var card in cartes)
        {
            if (card.Symbole is "Pique" or "Trefle") blacked.Add(card);
            else
            {
                rouged.Add(card);
            }
        }

        if (blacked.Count >= 5)
        {
            _combinaison += GetMaxPower(blacked);
            return true;
        }

        if (rouged.Count < 5) return false;
        _combinaison += GetMaxPower(rouged);
        return true;

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

    private static bool isQuinte(List<Cartes> cartes)
    {
        SortByPower(cartes);
        var suite = 0;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (cartes[i].Power - cartes[i + 1].Power == -1)
                suite++;
            else
                suite = 0;
            if (suite != 4) continue;
            _combinaison += cartes[i + 1].Power;
            return true;
        }

        foreach (var card in cartes.Where(card => card.Power == 14)) // LINQ : eq => pour tout les as 
            card.Power = 1;
        SortByPower(cartes);
        suite = 0;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (cartes[i].Power - cartes[i + 1].Power == -1)
                suite++;
            else
                suite = 0;
            if (suite != 4) continue;
            _combinaison += cartes[i + 1].Power;
            return true;
        }

        foreach (var card in cartes.Where(card => card.Power == 1)) // LINQ : eq => pour toutes les as je reset
            card.Power = 14;
        return false;
    }

    private static bool isBrelan(List<Cartes> cartes)
    {
        SortByPower(cartes);
        var same = 0;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (cartes[i].Power == cartes[i + 1].Power)
                same++;
            else
                same = 0;
            if (same != 2) continue;
            _combinaison += cartes[i + 1].Power;
            return true;
        }

        return false;
    }

    private static bool isTwoPairs(List<Cartes> cartes)
    {
        SortByPower(cartes);
        var nbPair = 0;
        for (var i = 0; i < cartes.Count - 1; i++)
        {
            if (cartes[i].Power == cartes[i + 1].Power) nbPair += 1;
            if (nbPair != 2) continue;
            _combinaison += cartes[i + 1].Power;
            return true;
        }

        return false;
    }

    private static bool isPair(List<Cartes> cartes)
    {
        SortByPower(cartes);
        for (var i = 0; i < cartes.Count - 1; i++)
            if (cartes[i].Power == cartes[i + 1].Power)
            {
                _combinaison += cartes[i + 1].Power;
                return true;
            }

        return false;
    }

    private static int isDefault(List<Cartes> cartes)
    {
        SortByPower(cartes);
        return cartes[^1].Power; // La derniere carte
    }

    #region StaticFunctions

    private static int GetMaxPower(List<Cartes> list)
    {
        var max = list[0].Power;
        for (var i = 1; i < list.Count; i++)
            if (list[i].Power > max)
                max = list[i].Power;
        return max;
    }

    private static void SortByPower(List<Cartes> cartes)
    {
        for (var i = 0; i < cartes.Count; i++)
        for (var j = i + 1; j < cartes.Count; j++)
            if (cartes[j].Power < cartes[i].Power)
                (cartes[j], cartes[i]) = (cartes[i], cartes[j]);
    }

    #endregion
}