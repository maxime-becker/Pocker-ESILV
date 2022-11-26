using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PockerApp
{
    internal class Player
    {
        public Cartes Card1 { get; set; }
        public Cartes Card2 { get; set; }
        public int Argent { get; set; }
        public bool CanPlay { get; set; }
        public string Name { get; set; }

        public Player(Cartes card1, Cartes card2, int argent, bool canPlay, string name)
        {
            Card1 = card1;
            Card2 = card2;
            Argent = argent;
            CanPlay = canPlay;
            Name = name.ToString();
        }
        public override string? ToString()
        {
            return "Carte 1 : " + Card1 + " / Carte 2 : " + Card2 + "  /  Argent : " + Argent + "  /  CanPlay ? : " + CanPlay + "  /  Nom : " + Name;
        }
    }
}
