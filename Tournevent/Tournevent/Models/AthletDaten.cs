using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class AthletDaten
    {
        public AthletDaten(Athleten athleten, int startnummer)
        {
            Id = athleten.Id;
            Vorname = athleten.Vorname;
            Nachname = athleten.Nachname;
            Jahrgang = athleten.Jahrgang;
            Startnummer = startnummer;
        }
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Jahrgang { get; set; }
        public int Startnummer { get; set; }
    }
}