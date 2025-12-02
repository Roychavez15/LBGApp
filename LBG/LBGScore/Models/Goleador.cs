using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class Goleador
    {
        public int Pos { get; set; }
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public string Goles { get; set; }
        public string Equipo { get; set; }
        public string Numero { get; set; }
        public string Foto { get; set; }
        public string Categoria { get; set; }
        public int GolesInt => int.TryParse(Goles, out var x) ? x : 0;
        public string Logo => $"http://181.39.104.93:5015/logo.aspx?id={Uri.EscapeDataString(Equipo)}";

    }
}
