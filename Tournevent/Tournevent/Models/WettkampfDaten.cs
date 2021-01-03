using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class WettkampfDaten
    {
        private readonly DBContext db = new DBContext();
        public WettkampfDaten()
        {

        }
        public WettkampfDaten(Wettkampf wettkampf)
        {
            wettkampfId = wettkampf.Id;
            WettkampfName = wettkampf.WettkampfName;
            WettkampfArt = wettkampf.Wettkampfart.Wettkampfart1;
            Datum = wettkampf.Datum;
        }
        public int wettkampfId { get; set; }
        [Required]
        [DisplayName("Wettkampf Name")]
        public string WettkampfName { get; set; }
        [Required]
        [DisplayName("Wettkampf Art")]
        public string WettkampfArt { get; set; }
        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Datum { get; set; }

        public void New()
        {
            Wettkampfart wettkampfart = (from wa in db.Wettkampfart where wa.Wettkampfart1 == WettkampfArt select wa).Single();
            Wettkampf wettkampf = new Wettkampf();
            wettkampf.WettkampfName = WettkampfName;
            wettkampf.Datum = Datum;
            wettkampf.WettkampfartId = wettkampfart.Id;
            db.Wettkampf.Add(wettkampf);
            db.SaveChanges();
        }
    }
}