using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinsverantwortlicherDaten
    {
        public VereinsverantwortlicherDaten()
        {

        }
        public VereinsverantwortlicherDaten(Benutzer benutzer)
        {
            using (DataContext db = new DataContext())
            {
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher
                        where v.Mailadresse == benutzer.Email
                        select v).SingleOrDefault();
                vereinsId = vereinsverantwortlicher.ID_Verein;
                Vorname = vereinsverantwortlicher.Vorname;
                Nachname = vereinsverantwortlicher.Nachname;
                Email = vereinsverantwortlicher.Mailadresse;
                Strasse = vereinsverantwortlicher.Adresse.Strasse;
                Hausnummer = vereinsverantwortlicher.Adresse.Hausnummer;
                Ort = vereinsverantwortlicher.Adresse.Ort;
                PLZ = vereinsverantwortlicher.Adresse.PLZ;
            }
        }
        public VereinsverantwortlicherDaten(Vereinsverantwortlicher vereinsverantwortlicher)
        {
            vereinsId = vereinsverantwortlicher.ID_Verein;
            Vorname = vereinsverantwortlicher.Vorname;
            Nachname = vereinsverantwortlicher.Nachname;
            Email = vereinsverantwortlicher.Mailadresse;
            Strasse = vereinsverantwortlicher.Adresse.Strasse;
            Hausnummer = vereinsverantwortlicher.Adresse.Hausnummer;
            Ort = vereinsverantwortlicher.Adresse.Ort;
            PLZ = vereinsverantwortlicher.Adresse.PLZ;
        }

        public void Update()
        {
            using (DataContext db = new DataContext())
            {
                Vereinsverantwortlicher vereinsverantwortlicher = (from v in db.Vereinsverantwortlicher where v.ID_Verein == vereinsId select v).Single();
                Adresse adresse = vereinsverantwortlicher.Adresse;
                vereinsverantwortlicher.Vorname = Vorname;
                vereinsverantwortlicher.Nachname = Nachname;
                vereinsverantwortlicher.Mailadresse = Email;
                adresse.Strasse = Strasse;
                adresse.PLZ = PLZ;
                adresse.Ort = Ort;
                adresse.Hausnummer = Hausnummer;
                vereinsverantwortlicher.Adresse = adresse;
                db.Vereinsverantwortlicher.Attach(vereinsverantwortlicher);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(vereinsverantwortlicher, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();
            }
        }
        public int vereinsId { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        public string Email { get; set; }
        [Required]
        public string Strasse { get; set; }
        [Required]
        public short Hausnummer { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public short PLZ { get; set; }
    }
}