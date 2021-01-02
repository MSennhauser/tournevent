using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class DisziplinenDaten
    {
        public DisziplinenDaten()
        {

        }
        public DisziplinenDaten(Disziplinen disziplinen)
        {
            disziplinenId = disziplinen.Id;
            Disziplin = disziplinen.Disziplin;
            Abkuerzung = disziplinen.Abkuerzung;
            Wettkampfart = disziplinen.Wettkampfart.Wettkampfart1;
            anzTeilnehmer = disziplinen.anzTeilnehmer != null ? (int)disziplinen.anzTeilnehmer : 0 ;
            Versuche = disziplinen.Versuche != null ? (int)disziplinen.Versuche : 0 ;
        }
        public int disziplinenId { get; set; }
        [Required]
        public string Disziplin { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        public string Wettkampfart { get; set; }
        [DisplayName("Teilnehmer")]
        public int anzTeilnehmer { get; set; }
        public int Versuche { get; set; }
    }
}