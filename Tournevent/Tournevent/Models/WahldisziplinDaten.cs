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
    public class WahldisziplinDaten
    {
        private readonly DataContext db = new DataContext();
        public WahldisziplinDaten()
        {
            AvailableDisziplinen = (from d in db.Disziplin where d.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select d).ToList();
        }
        public WahldisziplinDaten(Disziplin wahldisziplin)
        {
            Name = wahldisziplin.Name;
            wahldisziplinID = wahldisziplin.ID_Disziplin;
            wahldisziplinList = wahldisziplin.Wahldisziplin.ToList();
            AvailableDisziplinen = (from d in db.Disziplin where d.ID_Wettkampf == GlobalData.currentWettkampf.ID_Wettkampf select d).ToList();
        }
        public List<Wahldisziplin> wahldisziplinList { get; set; }
        public List<Disziplin> AvailableDisziplinen { get; set; }
        public int wahldisziplinID { get; set; }
        public int? ID_Wahldisziplin_Disziplin { get; set; }
        [Required]
        public string Name { get; set; }

        public void New()
        {
            Wahldisziplin wahldisziplin = new Wahldisziplin();
        }
        public void Update()
        {

        }
    }
}