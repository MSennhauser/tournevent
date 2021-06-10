using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class KontoDaten
    {
        public KontoDaten()
        {

        }
        public KontoDaten(Benutzer benutzer)
        {
            Email = benutzer.Email;
            Telefon = benutzer.Telefon;
            Passwort = benutzer.Passwort;
            ConfirmPasswort = benutzer.Passwort;
        }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"(\b(0041|0)|\B\+41)(\s?\(0\))?(\s)?[1-9]{2}(\s)?[0-9]{3}(\s)?[0-9]{2}(\s)?[0-9]{2}\b", ErrorMessage = "Keine gültige Telefon Nr.")]
        [DisplayName("Telefon Nr.")]
        public string Telefon { get; set; }
        [Required]
        public string Passwort { get; set; }
        [Required]
        [Compare("Passwort")]
        [DisplayName("Passwort bestätigen")]
        public string ConfirmPasswort { get; set; }
    }
}