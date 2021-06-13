using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class KategorienDaten
    {
        public KategorienDaten()
        {

        }
        public KategorienDaten(Kategorie kategorie)
        {
            kategorieID = kategorie.ID_Kategorie;
            Name = kategorie.Name;
            Abkuerzung = kategorie.Abkuerzung;
            JahrgangVon = kategorie.JahrgangVon;
            JahrgangBis = kategorie.JahrgangBis;
            Geschlecht = kategorie.Geschlecht;
        }
        public int kategorieID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        public int JahrgangVon { get; set; }
        [Required]
        public int JahrgangBis { get; set; }
        [Required]
        public string Geschlecht { get; set; }

        public void New()
        {
            using (DataContext db = new DataContext())
            {
                Kategorie kategorie = new Kategorie();
                Wettkampf wettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select w).Single();
                kategorie.Name = Name;
                kategorie.Abkuerzung = Abkuerzung;
                kategorie.JahrgangVon = (short)JahrgangVon;
                kategorie.JahrgangBis = (short)JahrgangBis;
                kategorie.Geschlecht = Geschlecht;
                kategorie.Wettkampf = wettkampf;
                db.Kategorie.Add(kategorie);
                db.SaveChanges();
            }
        }
        public void Update()
        {
            using (DataContext db = new DataContext())
            {
                Kategorie kategorie = (from k in db.Kategorie where k.ID_Kategorie == kategorieID select k).Single();
                kategorie.Name = Name;
                kategorie.Abkuerzung = Abkuerzung;
                kategorie.JahrgangVon = (short)JahrgangVon;
                kategorie.JahrgangBis = (short)JahrgangBis;
                kategorie.Geschlecht = Geschlecht;
                db.Kategorie.Attach(kategorie);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(kategorie, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();

            }
        }
    }
}