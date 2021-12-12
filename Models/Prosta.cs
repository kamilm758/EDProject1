using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDProject1.Models
{
    public class Prosta
    {
        public Prosta(bool kierunek, double wartoscCiecia, string nazwaZmiennej, string odcietaKlasaDecyzyjna)
        {
            Kierunek = kierunek;
            WartoscCiecia = wartoscCiecia;
            NazwaZmiennej = nazwaZmiennej;
            OdcietaKlasaDecyzyjna = odcietaKlasaDecyzyjna;
        }

        public bool Kierunek { get; set; }// true to mniejsze, jeżeli false to wieksze
        public double WartoscCiecia { get; set; }
        public string NazwaZmiennej { get; set; }
        public string OdcietaKlasaDecyzyjna { get; set; }
    }
}