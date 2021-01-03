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
        private readonly DBContext db = new DBContext();
        public AthletDaten()
        {

        }
        public AthletDaten(Athleten athleten, int startnummer)
        {
            Id = athleten.Id;
            Vorname = athleten.Vorname;
            Nachname = athleten.Nachname;
            Jahrgang = athleten.Jahrgang;
            GeschlechtId = athleten.Geschlechter.Index;
            Geschlecht = (from g in db.Geschlechter where g.Index == GeschlechtId select g.Definition).Single();
            Startnummer = startnummer;
        }
        public int Id { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        public int Jahrgang { get; set; }
        [DisplayName("Geschlecht")]
        [Required]
        public int GeschlechtId { get; set; }
        public string Geschlecht { get; set; }
        [Required]
        public int Startnummer { get; set; }

        public void New()
        {
            Athleten athlet = new Athleten();
            athlet.Vorname = Vorname;
            athlet.Nachname = Nachname;
            athlet.Jahrgang = Jahrgang;
            athlet.VereinsId = GlobalVariables.VereinsId;
            athlet.GeschlechtId = Convert.ToInt32(GeschlechtId);
            db.Athleten.Add(athlet);
            db.SaveChanges();

            Startnummern nr = new Startnummern();
            
            nr.AthletId = (from a in db.Athleten where a.Vorname == Vorname && a.Nachname == Nachname && a.Jahrgang == Jahrgang && a.VereinsId == GlobalVariables.VereinsId
                           select a.Id).Single();
            nr.WettkampfId = GlobalVariables.WettkampfId;
            nr.Startnummer = Startnummer;
            db.Startnummern.Add(nr);
            db.SaveChanges();
        }

        public void Update()
        {
            Athleten athlet = (from a in db.Athleten where a.Id == Id select a).Single();
            athlet.Vorname = Vorname;
            athlet.Nachname = Nachname;
            athlet.Jahrgang = Jahrgang;
            athlet.GeschlechtId = Convert.ToInt32(GeschlechtId);
            db.Athleten.Attach(athlet);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(athlet, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();

            Startnummern nr = (from s in db.Startnummern where s.AthletId == athlet.Id && s.WettkampfId == GlobalVariables.WettkampfId select s).Single();
            nr.Startnummer = Startnummer;
            db.Startnummern.Attach(nr);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(nr, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();
        }
    }
}