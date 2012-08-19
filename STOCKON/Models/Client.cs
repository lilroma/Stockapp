//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Client
    {
        public Client()
        {
            this.Facture_vente = new HashSet<Facture_vente>();
            this.Reglement = new HashSet<Reglement>();
        }
    
        public short Id_client { get; set; }
        [DisplayName("Nom")]
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom_client { get; set; }
        [DisplayName("Pr�nom")]
        public string Prenom_client { get; set; }
        [DisplayName("Cat�gorie")]
       // [Required(ErrorMessage = "La cat�gorie est obligatoire")]
        public string Categorie_client { get; set; }
        [DisplayName("Adresse")]
        public string Adresse_client { get; set; }
        [DisplayName("T�l�phone")]
        public string Telephone_client { get; set; }
        [DisplayName("EMail")]
        public string Mail_client { get; set; }
        [DisplayName("Code")]
        [Required(ErrorMessage = "Le colde est obligatoire")]
        public string Code_client { get; set; }
    
        public virtual ICollection<Facture_vente> Facture_vente { get; set; }
        public virtual ICollection<Reglement> Reglement { get; set; }
    }
}