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
    using System.Web.Script.Serialization;

    public partial class Liste_produit
    {
        [DisplayName("Article")]
        public short Id_article { get; set; }
        [DisplayName("Op�ration")]
        public short Id_operation { get; set; }
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
        public virtual Mouvement_stock Mouvement_stock { get; set; }
    }
    public partial class ProduitL
    {

        public short Id_article { get; set; }
        public short Id_operation { get; set; }
        public string Libelle_article { get; set; }
        public string Conditionnement { get; set; }
        public Nullable<int> Quantite { get; set; }
         
        public Nullable<decimal> Prix { get; set; }
        [UIHint("ListeArticle"), Required]
        public Arti Article { get; set; }
    }

    public partial class Arti
    {
        public short Id_article { get; set; }
        public string Libelle_article { get; set; }
    }
}
