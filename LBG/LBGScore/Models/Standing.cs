using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class Standing
    {
        public int Posicion { get; set; }
        public string Logo { get; set; }
        public string Equipo { get; set; }
        public int Pj { get; set; }
        public int Pg { get; set; }
        public int Pp { get; set; }
        public int Pe { get; set; }
        public int Gf { get; set; }
        public int Gc { get; set; }
        public int Puntos { get; set; }
        public int Gd { get; set; }
        public string Grupo { get; set; }
        public string Categoria { get; set; }
        public double Bonificacion { get; set; }
        public double Total { get; set; }
    }
}
