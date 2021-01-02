using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Vereins Name")]
        public string VereinsName { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        [RegularExpression(@"(\b(0041|0)|\B\+41)(\s?\(0\))?(\s)?[1-9]{2}(\s)?[0-9]{3}(\s)?[0-9]{2}(\s)?[0-9]{2}\b", ErrorMessage = "Keine gültige Telefon Nr.")]
        [DisplayName("Telefon Nr.")]
        public string Telefon { get; set; }

    }
}