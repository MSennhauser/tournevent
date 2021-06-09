using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        public void Update()
        {
            Vereinsverantwortlicher vereinsverantwortlicher = new Vereinsverantwortlicher();
            vereinsverantwortlicher.ID_Verein = vereinsId;
            vereinsverantwortlicher.Vorname = Vorname;
            vereinsverantwortlicher.Nachname = Nachname;
            vereinsverantwortlicher.Mailadresse = Email;
            vereinsverantwortlicher.Adresse.Strasse = Strasse;
            vereinsverantwortlicher.Adresse.Hausnummer = Hausnummer;
            vereinsverantwortlicher.Adresse.Ort = Ort;
            vereinsverantwortlicher.Adresse.PLZ = PLZ;
            using (DataContext db = new DataContext())
            {
                db.Vereinsverantwortlicher.Add(vereinsverantwortlicher);
                db.SaveChanges();
            }
        }
        public int vereinsId { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
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