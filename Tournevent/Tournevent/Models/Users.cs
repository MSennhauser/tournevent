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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.UserRolesMapping = new HashSet<UserRolesMapping>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DoesExistValidation(ErrorMessage = "Diese Email-Adresse hat bereits einen Account")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped] // Does not effect with your database
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolesMapping> UserRolesMapping { get; set; }
    }
}
