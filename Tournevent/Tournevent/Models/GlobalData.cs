using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public static class GlobalData
    {
        public static int WettkampfId { get; set; }
        public static int VereinsId { get; set; }
        public static Wettkampf currentWettkampf { get; set; }
        public static List<Wettkampf> wettkampfList { get; set; }
        public static Verein verein { get; set; }
    }
}