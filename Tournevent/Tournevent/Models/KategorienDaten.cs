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
        public KategorienDaten(Kategorie kategorie)
        {
            kategorieID = kategorie.ID_Kategorie;
            Kategorie = kategorie.Name;
            Abkuerzung = kategorie.Abkuerzung;
            JahrgangVon = kategorie.JahrgangVon;
            JahrgangBis = kategorie.JahrgangBis;
            Geschlecht = kategorie.Geschlecht;
        }
        public int kategorieID { get; set; }
        [Required]
        public string Kategorie { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        public int JahrgangVon { get; set; }
        [Required]
        public int JahrgangBis { get; set; }
        [Required]
        public string Geschlecht { get; set; }
    }
}