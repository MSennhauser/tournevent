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
    
    public partial class Wettkampf
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wettkampf()
        {
            this.Startnummern = new HashSet<Startnummern>();
            this.VereineWettkampf = new HashSet<VereineWettkampf>();
        }
    
        public int Id { get; set; }
        public string WettkampfName { get; set; }
        public int WettkampfartId { get; set; }
        public System.DateTime Datum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Startnummern> Startnummern { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VereineWettkampf> VereineWettkampf { get; set; }
        public virtual Wettkampfart Wettkampfart { get; set; }
    }
}
