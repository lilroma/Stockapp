﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STOCKON.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class STOCKONEntities : DbContext
    {
        public STOCKONEntities()
            : base("name=STOCKONEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Article> Article { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Facture_achat> Facture_achat { get; set; }
        public DbSet<Facture_vente> Facture_vente { get; set; }
        public DbSet<Famille_article> Famille_article { get; set; }
        public DbSet<Fournisseur> Fournisseur { get; set; }
        public DbSet<Liste_produit> Liste_produit { get; set; }
        public DbSet<Liste_produit_achat> Liste_produit_achat { get; set; }
        public DbSet<Liste_produit_vente> Liste_produit_vente { get; set; }
        public DbSet<Mouvement_stock> Mouvement_stock { get; set; }
        public DbSet<Reglement> Reglement { get; set; }
        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<Preference> Preference { get; set; }
        public DbSet<Log> Log { get; set; }
    }
}