using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STOCKON.Models
{
    public class ModelFacture
    {
        public string Nom_E { get; set; }
        public string Adresse_E { get; set; }
        public string Numtel_E { get; set; }
        public string Date_Facture { get; set; }
        public string Num_Facture { get; set; }
        public string Nom_client { get; set; }
        public string Adresse_client { get; set; }
        public string Telephone_client { get; set; }
        public string Type_Op { get; set; }
        public double Remise { get; set; }
        public string TRemise { get; set; }
        public Boolean TVA { get; set; }
        public double TauxTva { get; set; }
        public virtual List<Liste_produit_achat> Liste_produit_achat { get; set; }
        public virtual List<Liste_produit> Liste_produit { get; set; }
        public virtual List<Liste_produit_vente> Liste_produit_vente { get; set; }

    }
}