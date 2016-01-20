using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CrudCommande;

namespace ServiceCommande
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ICommandeService
    {
        [OperationContract]
        List<Commande> GetCommandeList();

        [OperationContract]
        string AddCommande(string StatutCommande, string IdDonneurOrdre, DateTime DateCommande);

        [OperationContract]
        string ModifyCommande(Commande uneCommande);

        [OperationContract]
        bool RemoveCommande(Commande uneCommande);

        [OperationContract]
        int InsertNumSerie(string NumSeries, string IdEmplacement);



        // TODO: ajoutez vos opérations de service ici
    }
}
