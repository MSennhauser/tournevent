using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class DisziplinenDaten
    {
        private readonly DataContext db = new DataContext();
        public DisziplinenDaten()
        {
            AvailableWahldisziplinen = new List<Disziplin>();
        }
        public DisziplinenDaten(Disziplin disziplin)
        {
            disziplinenId = disziplin.ID_Disziplin;
            Name = disziplin.Name;
            Abkuerzung = disziplin.Abkuerzung;
            ZeitTeilnehmer = disziplin.ZeitTeilnehmer;
            AnzahlVersuche = disziplin.AnzahlVersuche;

            AvailableWahldisziplinen = (from d in db.Disziplin where d.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf && d.ID_Disziplin != disziplinenId select d).ToList();
        }
        public int disziplinenId { get; set; }
        public List<Disziplin> AvailableWahldisziplinen { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [DisplayName("AnzahlVersuche")]
        public int? AnzahlVersuche { get; set; }
        public short? ZeitTeilnehmer { get; set; }

        public void New()
        {
            using (DataContext db = new DataContext())
            {
                Disziplin disziplin = new Disziplin();
                Wettkampf wettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select w).Single();
                disziplin.Name = Name;
                disziplin.Abkuerzung = Abkuerzung;
                disziplin.AnzahlVersuche = AnzahlVersuche;
                disziplin.ZeitTeilnehmer = ZeitTeilnehmer;
                disziplin.Wettkampf = wettkampf;
                db.Disziplin.Add(disziplin);
                db.SaveChanges();
            }
        }
        public void Update()
        {
            using (DataContext db = new DataContext())
            {
                Disziplin disziplin = (from d in db.Disziplin where d.ID_Disziplin == disziplinenId select d).Single();
                disziplin.Name = Name;
                disziplin.Abkuerzung = Abkuerzung;
                disziplin.AnzahlVersuche = AnzahlVersuche;
                disziplin.ZeitTeilnehmer = ZeitTeilnehmer;
                db.Disziplin.Attach(disziplin);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(disziplin, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();

            }
        }
    }
}