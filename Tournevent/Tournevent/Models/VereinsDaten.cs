using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinsDaten
    {
        public VereinsDaten()
        {

        }
        public VereinsDaten(Benutzer benutzer, Verein verein)
        {
            userId = benutzer.Id;
            VereinsName = verein.Vereinsname;
            Vorname = benutzer.Vorname;
            Nachname = benutzer.Nachname;
            Telefon = benutzer.Telefon;
        }
        public int userId { get; set; }
        [Required]
        public string VereinsName { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        public string Telefon { get; set; }

    }
}