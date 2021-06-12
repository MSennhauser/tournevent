using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinsDaten
    {
        public VereinsDaten()
        {

        }
        public VereinsDaten(Verein verein)
        {
            vereinsId = verein.ID_Verein;
            VereinsName = verein.Name;
            Ort = verein.Ort;
            PLZ = verein.PLZ;
        }
        public void Update()
        {
            using (DataContext db = new DataContext())
            {
                Verein verein = (from v in db.Verein where v.ID_Verein == vereinsId select v).Single();
                verein.Name = VereinsName;
                verein.Ort = Ort;
                verein.PLZ = PLZ;
                db.Verein.Attach(verein);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(verein, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();

            } 
        }
        public int vereinsId { get; set; }
        [Required]
        [DisplayName("Vereins Name")]
        public string VereinsName { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public int PLZ { get; set; }       

    }
}