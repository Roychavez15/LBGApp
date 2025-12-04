using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class Auspiciante
    {
        public string Archivo { get; set; }

        public string BannerUrl =>
            $"http://181.39.104.93:5015/banners/{Archivo}";
    }
}
