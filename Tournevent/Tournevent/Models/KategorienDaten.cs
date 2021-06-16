using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tournevent.Models
{
    public class KategorienDaten
    {
        private readonly DataContext db = new DataContext();
        public KategorienDaten()
        {
            SetDisziplinen();
        }
        public KategorienDaten(Kategorie kategorie)
        {
            kategorieID = kategorie.ID_Kategorie;
            Name = kategorie.Name;
            Abkuerzung = kategorie.Abkuerzung;
            JahrgangVon = kategorie.JahrgangVon;
            JahrgangBis = kategorie.JahrgangBis;
            Geschlecht = kategorie.Geschlecht;
            SetDisziplinen();
        }
        public int kategorieID { get; set; }
        public List<Disziplin> AvailableDisziplinen { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        [DisplayName("Jahrgang Von")]
        public int JahrgangVon { get; set; }
        [Required]
        [DisplayName("Jahrgang Bis")]
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
        private void SetDisziplinen()
        {
                AvailableDisziplinen = (from d in db.Disziplin where d.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select d).ToList();
        }
    }
}