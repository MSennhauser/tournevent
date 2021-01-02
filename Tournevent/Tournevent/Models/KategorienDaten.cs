using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class KategorienDaten
    {
        public KategorienDaten()
        {

        }
        public KategorienDaten(Kategorien kategorien, Geschlechter geschlecht)
        {
            kategorieID = kategorien.Id;
            Kategorie = kategorien.Kategorie;
            Abkuerzung = kategorien.Abkuerzung;
            Jahrgang = kategorien.Jahrgang != null ? (int)kategorien.Jahrgang : 0;
            Geschlecht = geschlecht.Bezeichnung;
        }
        public int kategorieID { get; set; }
        [Required]
        public string Kategorie { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        public int Jahrgang { get; set; }
        [Required]
        public string Geschlecht { get; set; }

    }
}