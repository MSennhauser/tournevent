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
        public DisziplinenDaten(Disziplin disziplin)
        {
            disziplinenId = disziplin.ID_Disziplin;
            Disziplin = disziplin.Name;
            Abkuerzung = disziplin.Abkuerzung;
            ZeitTeilnehmer = disziplin.ZeitTeilnehmer;
            AnzahlVersuche = disziplin.AnzahlVersuche;
        }
        public int disziplinenId { get; set; }
        [Required]
        public string Disziplin { get; set; }
        [Required]
        [DisplayName("Abkürzung")]
        public string Abkuerzung { get; set; }
        [Required]
        public string Wettkampfart { get; set; }
        [DisplayName("AnzahlVersuche")]
        public int AnzahlVersuche { get; set; }
        public int Versuche { get; set; }
        public short? ZeitTeilnehmer { get; set; }
    }
}