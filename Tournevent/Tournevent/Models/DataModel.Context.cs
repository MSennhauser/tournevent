﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base(TourneventContext.Connect())
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Anmeldung> Anmeldung { get; set; }
        public virtual DbSet<Athlet> Athlet { get; set; }
        public virtual DbSet<Benutzer> Benutzer { get; set; }
        public virtual DbSet<Kategorie> Kategorie { get; set; }
        public virtual DbSet<Startnummer> Startnummer { get; set; }
        public virtual DbSet<Verein> Verein { get; set; }
        public virtual DbSet<Vereinsverantwortlicher> Vereinsverantwortlicher { get; set; }
        public virtual DbSet<Wettkampf> Wettkampf { get; set; }
        public virtual DbSet<Adresse> Adresse { get; set; }
        public virtual DbSet<Disziplin> Disziplin { get; set; }
        public virtual DbSet<Kategorie_Disziplin> Kategorie_Disziplin { get; set; }
        public virtual DbSet<Wahldisziplin> Wahldisziplin { get; set; }
    }
}
