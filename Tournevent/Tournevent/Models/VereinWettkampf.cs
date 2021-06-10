using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinWettkampf
    {
        public VereinWettkampf()
        {

        }
        public VereinWettkampf(Verein verein, List<int> wettkampfId)
        {
            vereinId = verein.ID_Verein;
            VereinsName = verein.Name;
            WettkampfIds = wettkampfId;
        }
        public int vereinId { get; set; }
        [Required]
        public string VereinsName { get; set; }
        [Required]
        public List<int> WettkampfIds { get; set; }
    }
}