using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class AthletDaten
    {
        private readonly DataContext db = new DataContext();
        public AthletDaten()
        {

        }
        public AthletDaten(Athlet athlet, int startnummer)
        {
            Id = athlet.ID_Athlet;
            Vorname = athlet.Vorname;
            Nachname = athlet.Nachname;
            Geburtsdatum = athlet.Geburtsdatum;
            Geschlecht = athlet.Geschlecht;
            Startnummer = startnummer;
            Strasse = athlet.Adresse.Strasse;
            Hausnummer = athlet.Adresse.Hausnummer;
            Ort = athlet.Adresse.Ort;
            PLZ = athlet.Adresse.PLZ;
        }
        public int Id { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        public DateTime Geburtsdatum { get; set; }
        [Required]
        public string Geschlecht { get; set; }
        [Required]
        public int Startnummer { get; set; }
        [Required]
        public string Strasse { get; set; }
        [Required]
        public short Hausnummer { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public short PLZ { get; set; }

        public void New()
        {
            Athlet athlet = new Athlet();
            Adresse adresse = new Adresse();
            athlet.Vorname = Vorname;
            athlet.Nachname = Nachname;
            athlet.Geburtsdatum = Geburtsdatum;
            athlet.ID_Verein = GlobalData.verein.ID_Verein;
            athlet.Geschlecht = Geschlecht;
            // Add Adresse
            adresse.Strasse = Strasse;
            adresse.PLZ = PLZ;
            adresse.Ort = Ort;
            adresse.Hausnummer = Hausnummer;
            athlet.Adresse = adresse;
            db.Athlet.Add(athlet);
            db.SaveChanges();

            Startnummer nr = new Startnummer();
            
            nr.ID_Athlet = (from a in db.Athlet where a.Vorname == Vorname && a.Nachname == Nachname && a.Geburtsdatum == Geburtsdatum && a.ID_Verein == GlobalData.verein.ID_Verein
                            select a.ID_Athlet).Single();
            nr.ID_Wettkampf = GlobalData.currentWettkampf.ID_Wettkampf;
            nr.Startnr = Startnummer;
            db.Startnummer.Add(nr);
            db.SaveChanges();
        }

        public void Update()
        {
            Athlet athlet = (from a in db.Athlet where a.ID_Athlet == Id select a).Single();
            Adresse adresse = athlet.Adresse;
            athlet.Vorname = Vorname;
            athlet.Nachname = Nachname;
            athlet.Geburtsdatum = Geburtsdatum;
            athlet.Geschlecht = Geschlecht;
            adresse.Strasse = Strasse;
            adresse.PLZ = PLZ;
            adresse.Ort = Ort;
            adresse.Hausnummer = Hausnummer;
            athlet.Adresse = adresse;
            db.Athlet.Attach(athlet);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(athlet, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();

            Startnummer nr = (from s in db.Startnummer where s.ID_Athlet == athlet.ID_Athlet && s.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select s).Single();
            nr.Startnr = Startnummer;
            db.Startnummer.Attach(nr);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(nr, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();
        }
    }
}