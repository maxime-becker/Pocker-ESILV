using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PockerApp
{
    internal class Cartes
    {
        public string Number { get; set; }
        public string Color { get; set; }
        public string Symbole { get; set; }

        public int Id { get; set; }
        public Cartes(string number, string color, string symbole, int id)
        {
            Number = number;
            Color = color;
            Symbole = symbole;
            Id = id;



        }
    }
}
