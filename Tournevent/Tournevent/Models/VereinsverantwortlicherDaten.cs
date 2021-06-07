using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class VereinsverantwortlicherDaten
    {
        public int vereinsId { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        [Required]
        public string Strasse { get; set; }
        [Required]
        public short Hausnummer { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public short PLZ { get; set; }
    }
}