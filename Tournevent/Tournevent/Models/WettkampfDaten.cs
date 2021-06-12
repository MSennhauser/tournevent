using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class WettkampfDaten
    {
        private readonly DataContext db = new DataContext();
        public WettkampfDaten()
        {

        }
        public WettkampfDaten(Wettkampf wettkampf)
        {
            wettkampfId = wettkampf.ID_Wettkampf;
            WettkampfName = wettkampf.Name;
            Datum = wettkampf.Datum;
            TeilnahmeBeginn = wettkampf.Teilnahmebeginn;
            TeilnahmeSchluss = wettkampf.Teilnahmeschluss;
            NummerVon = wettkampf.NummerVon;
            NummerBis = wettkampf.NummerBis;
        }
        public int wettkampfId { get; set; }
        [Required]
        [DisplayName("Wettkampf Name")]
        public string WettkampfName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Datum { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TeilnahmeBeginn { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TeilnahmeSchluss { get; set; }
        [Required]
        public int NummerVon { get; set; }
        [Required]
        public int NummerBis { get; set; }

        public void New(string email)
        {
            Wettkampf wettkampf = new Wettkampf();
            Administrator admin = (from a in db.Administrator where a.Mailadresse == email select a).FirstOrDefault();
            wettkampf.Name = WettkampfName;
            wettkampf.Datum = Datum;
            wettkampf.Teilnahmebeginn = TeilnahmeBeginn;
            wettkampf.Teilnahmeschluss = TeilnahmeSchluss;
            wettkampf.NummerVon = NummerVon;
            wettkampf.NummerBis = NummerBis;
            wettkampf.Administrator = admin;
            db.Wettkampf.Add(wettkampf);
            db.SaveChanges();
        }
        public void Update()
        {
            using (DataContext db = new DataContext())
            {
                Wettkampf wettkampf = (from w in db.Wettkampf where w.ID_Wettkampf == wettkampfId select w).Single();
                wettkampf.Name = WettkampfName;
                wettkampf.Datum = Datum;
                wettkampf.Teilnahmebeginn = TeilnahmeBeginn;
                wettkampf.Teilnahmeschluss = TeilnahmeSchluss;
                wettkampf.NummerVon = NummerVon;
                wettkampf.NummerBis = NummerBis;
                db.Wettkampf.Attach(wettkampf);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(wettkampf, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();

            }
        }
    }
}