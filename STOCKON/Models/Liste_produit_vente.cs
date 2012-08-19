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
    using System.Web.Script.Serialization;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Liste_produit_vente
    {
        [DisplayName("Article")]
        public short Id_article { get; set; }
        [DisplayName("Facture")]
        public short Id_facture { get; set; }
        [Required(ErrorMessage = "La quantit� est obligatoire")]
        [DisplayName("Quantit�")]
        public Nullable<int> Quantite { get; set; }
        [Required(ErrorMessage = "Le prix est obligatoire")]
        [DisplayName("PU")]
        public Nullable<decimal> Prix { get; set; }
        [DisplayName("PT")]
        public Nullable<decimal> Prix_Total { get; set; }
        [ScriptIgnore]
        [UIHint("ListeArticle")]
        public virtual Article Article { get; set; }
        [ScriptIgnore]
        public virtual Facture_vente Facture_vente { get; set; }
    }

  /*  public class ValidQte : ValidationAttribute
    {
        private STOCKONEntities db = new STOCKONEntities();

        public override bool IsValid(object value)
        {
            
        }
    
    }*/
}
