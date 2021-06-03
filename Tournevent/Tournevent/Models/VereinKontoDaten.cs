using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinKontoDaten
    {
        private readonly DataContext db = new DataContext();
        public VereinKontoDaten()
        {

        }
        public VereinKontoDaten(Benutzer benutzer)
        {
            userId = benutzer.ID_Benutzer;
            Email = benutzer.Email;
            Passwort = benutzer.Passwort;
            ConfirmPasswort = Passwort;
            Telefon = benutzer.Telefon;
        }
        public int userId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passwort { get; set; }
        [Required]
        [Compare("Passwort")]
        [DisplayName("Passwort bestätigen")]
        public string ConfirmPasswort { get; set; }
        [Required]
        [RegularExpression(@"(\b(0041|0)|\B\+41)(\s?\(0\))?(\s)?[1-9]{2}(\s)?[0-9]{3}(\s)?[0-9]{2}(\s)?[0-9]{2}\b", ErrorMessage = "Keine gültige Telefon Nr.")]
        [DisplayName("Telefon Nr.")]
        public string Telefon { get; set; }

        public void Update()
        {
            Benutzer benutzer = (from b in db.Benutzer
                                 where b.ID_Benutzer == userId
                                 select b).Single();
            benutzer.Telefon = Telefon;
            benutzer.Email = Email;
            benutzer.Passwort = Passwort;

            db.Benutzer.Attach(benutzer);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);

            db.SaveChanges();
        }
    }
}