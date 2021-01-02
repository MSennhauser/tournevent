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
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passwort { get; set; }
        [Required]
        [Compare("Passwort")]
        [DisplayName("Passwort bestätigen")]
        public string ConfirmPasswort { get; set; }
    }
}