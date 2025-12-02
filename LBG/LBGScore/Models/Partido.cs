using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class Partido
    {
        public string Dia { get; set; }          // "Dom. 30 Nov."
        public string Hora { get; set; }         // "10:00"
        public string Local { get; set; }
        public string Visitante { get; set; }
        public string GolesLocal { get; set; }
        public string GolesVisitante { get; set; }
        public string Categoria { get; set; }
        public string Vocal { get; set; }
        public string Tiempo { get; set; }
        public string Estado { get; set; }       // 0,1,2
        public string Jugado { get; set; }
        public string GolesL { get; set; }
        public string GolesV { get; set; }
        public string LogoL { get; set; }        // url logo local
        public string LogoV { get; set; }        // url logo visitante

        // ===========================
        // Helpers
        // ===========================

        public int GL => int.TryParse(GolesLocal ?? GolesL, out var x) ? x : 0;
        public int GV => int.TryParse(GolesVisitante ?? GolesV, out var x) ? x : 0;

        public string EstadoTexto =>
            Estado switch
            {
                "0" => "Pendiente",
                "1" => "En juego",
                "2" => "Finalizado",
                _ => ""
            };

        public string EstadoColorHex =>
            Estado switch
            {
                "0" => "#777777", // gris
                "1" => "#E53935", // rojo
                "2" => "#2E7D32", // verde
                _ => "#000000"
            };

        // Normalización ordenable
        public DateTime FechaHoraOrdenable
        {
            get
            {
                // Ejemplo Dia: "Dom. 30 Nov."
                // Lo convertimos a fecha ignorando nombre día
                try
                {
                    var partes = Dia.Replace(".", "").Split(' ');
                    // partes = ["Dom", "30", "Nov"]

                    int dia = int.Parse(partes[1]);
                    string mesStr = partes[2].ToLower();

                    int mes = mesStr switch
                    {
                        "ene" => 1,
                        "feb" => 2,
                        "mar" => 3,
                        "abr" => 4,
                        "may" => 5,
                        "jun" => 6,
                        "jul" => 7,
                        "ago" => 8,
                        "sep" => 9,
                        "oct" => 10,
                        "nov" => 11,
                        "dic" => 12,
                        _ => 1
                    };

                    var partesHora = Hora.Split(':');
                    int h = int.Parse(partesHora[0]);
                    int m = int.Parse(partesHora[1]);

                    // Año actual
                    return new DateTime(DateTime.Now.Year, mes, dia, h, m, 0);
                }
                catch
                {
                    return DateTime.MaxValue; // si falla, mandarlo al final
                }
            }
        }
    }


}
