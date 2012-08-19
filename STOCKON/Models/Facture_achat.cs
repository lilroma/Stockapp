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
    
    public partial class Facture_achat
    {
        public Facture_achat()
        {
            this.Liste_produit_achat = new HashSet<Liste_produit_achat>();
        }
    
        public short Id_facture { get; set; }
        [DisplayName("Fournisseur")]
       // [Required(ErrorMessage = "Le fournisseur est obligatoire")]
        public Nullable<short> Id_fournisseur { get; set; }
        [DisplayName("Date")]
        [Required(ErrorMessage = "La date est obligatoire")]
        public System.DateTime Date_facture { get; set; }
        [DisplayName("Mode de r�glement")]
        //[Required(ErrorMessage = "Le mode de r�glement est obligatoire")]
        public string Mode_reglement { get; set; }
        [DisplayName("Type")]
        //[Required(ErrorMessage = "Le type est obligatoire")]
        public string Type_facture { get; set; }
       // public string Numero_facture { get; set; }
        [DisplayName("Code")]
        [Required(ErrorMessage = "Le colde est obligatoire")]
        public string Code_facture { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }
        public virtual ICollection<Liste_produit_achat> Liste_produit_achat { get; set; }
    }
}