using System;
using System.Collections.Generic;
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
            Startnummer = startnummer;
        }
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Jahrgang { get; set; }
        public int Startnummer { get; set; }

        public void New()
        {
            Athleten athlet = new Athleten();
            athlet.Vorname = Vorname;
            athlet.Nachname = Nachname;
            athlet.Jahrgang = Jahrgang;
            athlet.VereinsId = GlobalVariables.VereinsId;
            db.Athleten.Add(athlet);
            db.SaveChanges();

            Startnummern nr = new Startnummern();
            nr.AthletId = (from a in db.Athleten where a.Vorname == Vorname && a.Nachname == Nachname && a.Jahrgang == Jahrgang
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
            db.Athleten.Attach(athlet);
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(athlet, System.Data.Entity.EntityState.Modified);
            db.SaveChanges();

        }
    }
}