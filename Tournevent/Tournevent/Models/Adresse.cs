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
    
    public partial class Adresse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adresse()
        {
            this.Athlet = new HashSet<Athlet>();
            this.Vereinsverantwortlicher = new HashSet<Vereinsverantwortlicher>();
        }
    
        public int ID_Adresse { get; set; }
        public string Strasse { get; set; }
        public string Ort { get; set; }
        public short PLZ { get; set; }
        public string Hausnummer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Athlet> Athlet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vereinsverantwortlicher> Vereinsverantwortlicher { get; set; }
    }
}
