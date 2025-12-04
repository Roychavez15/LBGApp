using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class LiveMatch
    {
        public string Dia { get; set; }
        public string Hora { get; set; }
        public string Local { get; set; }
        public string Visitante { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public string Categoria { get; set; }
        public string Tiempo { get; set; }
        public string LogoL { get; set; }
        public string LogoV { get; set; }
    }
}
