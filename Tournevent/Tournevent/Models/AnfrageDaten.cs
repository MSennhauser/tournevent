using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class AnfrageDaten
    {
        public AnfrageDaten()
        {

        }
        public AnfrageDaten(Vereinsverantwortlicher vereinsverantwortlicher)
        {
            VereinsName = vereinsverantwortlicher.Verein.Name;
            Email = vereinsverantwortlicher.Mailadresse;
            Vorname = vereinsverantwortlicher.Vorname;
            Nachname = vereinsverantwortlicher.Nachname;
            using (DataContext db = new DataContext())
            {
                userId = db.Benutzer.FirstOrDefault(u => u.Email == vereinsverantwortlicher.Mailadresse).ID_Benutzer;
            }
        }
        public int userId { get; set; }
        [Required]
        [DisplayName("Vereins Name")]
        public string VereinsName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
    }
}