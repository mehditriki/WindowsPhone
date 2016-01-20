using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CrudCommande;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceCommande
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    [ServiceBehavior(IgnoreExtensionDataObject = false)]
    public class CommandeService : ICommandeService
    {
        CommandeDAO DAO = new CommandeDAO();

        public List<Commande> GetCommandeList()
        {
            return DAO.GetAllCommande();
        }
        public string AddCommande(string StatutCommande, string IdDonneurOrdre, DateTime DateCommande)
        {
            Commande NouvelleCommande = new Commande();
            NouvelleCommande.StatutCommande = StatutCommande;
            NouvelleCommande.IdDonneurOrdre = IdDonneurOrdre;
            NouvelleCommande.DateCommande = DateCommande;
            return DAO.AddCommande(NouvelleCommande);
           
        }
        public string ModifyCommande(Commande uneCommande)
        {
            return DAO.ModifyCommande(uneCommande);
        }
        public bool RemoveCommande(Commande uneCommande)
        {
            return DAO.DeleteCommande(uneCommande);
        }
        public int InsertNumSerie(string NumSeries, string IdEmplacement)
        {
            return DAO.insertNumSerie(NumSeries, IdEmplacement);
        }
    }
}
