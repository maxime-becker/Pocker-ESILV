using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Cartes(string number, string color, string symbole)
        {
            Number = number;
            Color = color;
            Symbole = symbole;
        }
    }
    
}
