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
    
    public partial class Reglement
    {
        [DisplayName("La num�ro de la facture est obligatoire")]
        public short Id_facture { get; set; }
        [Required(ErrorMessage = "Le client est obligatoire")]
        public short Id_client { get; set; }
        [DisplayName("Date")]
        [Required(ErrorMessage = "la date est ob;ligatoire")]
        public System.DateTime Date_regelement { get; set; }
        [DisplayName("Montant")]
        [Required(ErrorMessage = "Le montant est obligatoire")]
        public string Montant_regelement { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Facture_vente Facture_vente { get; set; }
    }
}
