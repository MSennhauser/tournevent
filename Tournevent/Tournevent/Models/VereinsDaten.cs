using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            VereinsName = verein.Name;
            Ort = verein.Ort;
            PLZ = verein.PLZ;
        }
        [Required]
        [DisplayName("Vereins Name")]
        public string VereinsName { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public int PLZ { get; set; }       

    }
}