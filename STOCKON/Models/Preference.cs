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
    
    public partial class Preference
    {
        [DisplayName("Raison sociale")]        
        public string Nom { get; set; }
        [DisplayName("Activit�")]
        public string Activite { get; set; }
        public string Adresse { get; set; }
        [DisplayName("Num�ro de t�l�phone")]
        public string Num_telephone { get; set; }
        [DisplayName("Type d'entreprise")]
        public string Type { get; set; }
        [DisplayName("Taux de la TVA appliqu�")]
        public Nullable<double> Taux_tva { get; set; }
        [DisplayName("Durr�e des �ch�ances clients")]
        public Nullable<int> Duree_echeance { get; set; }
        public short Id { get; set; }
        [DisplayName("Crit�re de recherche [Facturation]")]
        public string CRecherche { get; set; }
        [DisplayName("Mode de remise")]
        public string Remise { get; set; }
    }
}
