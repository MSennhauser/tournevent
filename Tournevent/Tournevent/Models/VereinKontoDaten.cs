using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinKontoDaten
    {
        private readonly DBContext db = new DBContext();
        public VereinKontoDaten()
        {

        }
        public VereinKontoDaten(Benutzer benutzer, Verein verein)
        {
            userId = benutzer.Id;
            Email = benutzer.Email;
            Passwort = benutzer.Passwort;
            ConfirmPasswort = Passwort;
            VereinsName = verein.Vereinsname;
            Vorname = benutzer.Vorname;
            Nachname = benutzer.Nachname;
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
        public string ConfirmPasswort { get; set; }
        [Required]
        public string VereinsName { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        [RegularExpression(@"(\b(0041|0)|\B\+41)(\s?\(0\))?(\s)?[1-9]{2}(\s)?[0-9]{3}(\s)?[0-9]{2}(\s)?[0-9]{2}\b", ErrorMessage = "Not a valid phone number")]
        public string Telefon { get; set; }

        public void Update()
        {
            Verein verein = (from v in db.Verein join b in db.Benutzer on v.Index equals b.VereinId where b.Id == userId select v).Single();
            verein.Vereinsname = VereinsName;
            db.Verein.Attach(verein);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(verein, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();

            Benutzer benutzer = (from b in db.Benutzer
                                 where b.Id == userId
                                 select b).Single();
            benutzer.Telefon = Telefon;
            benutzer.Nachname = Nachname;
            benutzer.Vorname = Vorname;
            benutzer.Email = Email;
            benutzer.Passwort = Passwort;

            db.Benutzer.Attach(benutzer);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);

            db.SaveChanges();
        }
    }
}