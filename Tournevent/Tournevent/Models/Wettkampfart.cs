//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tournevent.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Wettkampfart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wettkampfart()
        {
            this.Disziplinen = new HashSet<Disziplinen>();
            this.DisziplinenAuswahl_Beschreibung = new HashSet<DisziplinenAuswahl_Beschreibung>();
        }
    
        public int Index { get; set; }
        public string Wettkampfart1 { get; set; }
        public string Abkürzung { get; set; }
        public Nullable<bool> Einzelwettkampf { get; set; }
        public Nullable<bool> JedenTeilnehmer_erfassen { get; set; }
        public Nullable<short> AnzahlTeilnehmerMind { get; set; }
        public Nullable<short> AnzahlTeilnehmerMax { get; set; }
        public Nullable<bool> RangierungAbgeschlossen { get; set; }
        public string RanglisteAusgabeformat { get; set; }
        public Nullable<bool> Mit_Jahrgang { get; set; }
        public Nullable<System.DateTime> Erstellt_am { get; set; }
        public string Erstellt_von { get; set; }
        public Nullable<System.DateTime> Letzte_Änderung { get; set; }
        public string Zuletzt_gespeichert { get; set; }
        public byte[] SSMA_TimeStamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disziplinen> Disziplinen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisziplinenAuswahl_Beschreibung> DisziplinenAuswahl_Beschreibung { get; set; }
    }
}
