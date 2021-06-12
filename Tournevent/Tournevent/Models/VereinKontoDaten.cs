using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinKontoDaten
    {
        public VereinKontoDaten()
        {

        }
        public VereinsverantwortlicherDaten VereinsverantwortlicherDaten { get; set; }
        public VereinsDaten VereinsDaten { get; set; }
        public KontoDaten KontoDaten { get; set; }
    }
}