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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using System.ComponentModel;
    
    public partial class Utilisateur
    {
        public short Id_utilisateur { get; set; }
        [Required(ErrorMessage = "Le login est obligatoire")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        public string Pasword { get; set; }
        [Compare("Pasword", ErrorMessage = "Le nouveau mot de passe et celui de confirmation sont diff�rents")]
        public string ConfirmPassword { get; set; }
        [Required (ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Client { get; set; }
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Fournissaeur { get; set; }
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Facture { get; set; }
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Article { get; set; }
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Mouvement_stock { get; set; }
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> Etats { get; set; }
        [DisplayName("Edition montant")]
        [Required(ErrorMessage = "D�finir cette valeur")]
        public Nullable<bool> PFacture { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Ancien mot de passe")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation mot de passe")]
        [Compare("NewPassword", ErrorMessage = "le nouveau mot de passe et celui de confirmation sont diff�rents")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}
