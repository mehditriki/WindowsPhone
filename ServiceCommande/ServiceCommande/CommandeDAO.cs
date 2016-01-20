using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrudCommande;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Npgsql;
using System.Configuration;
namespace ServiceCommande
{
    public class CommandeDAO
    {
        CrudCommandeContext context = new CrudCommandeContext();
        public List<ValidationResult> validerChamps(Commande obj)
        {
            ValidationContext context = new ValidationContext(obj, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, context, results, true);
            return results;
        }


        public List<Commande> GetAllCommande()
        {
            var query = from it in context.Commandes
                        orderby it.DateCommande
                        select it;
            List<Commande> listCommande=new List<Commande>();
            foreach (CrudCommande.Commande com in query)
                listCommande.Add(com);
            return listCommande;
        }
        public string AddCommande(Commande NvelleCommande)
        {
            string Msg = "";
            List<ValidationResult> results = new List<ValidationResult>();
            results = validerChamps(NvelleCommande);
            if (results.Count == 0)
            {
                context.Commandes.InsertOnSubmit(NvelleCommande);
                context.SubmitChanges();
            }
            else
            {
                foreach(ValidationResult result in results)
                {
                    Msg += result.ErrorMessage + "\n";
                }
            }
            
            return Msg;
        }
        public string ModifyCommande(Commande Commande)
        {
            string Msg = "";
            List<ValidationResult> results = new List<ValidationResult>();
            results = validerChamps(Commande);
            if (results.Count == 0)
            {
                var query = from it in context.Commandes
                            where it.Id == Commande.Id
                            select it;
                foreach (Commande cmd in query)
                {
                    cmd.DateCommande = Commande.DateCommande;
                    cmd.IdDonneurOrdre = Commande.IdDonneurOrdre;
                    cmd.StatutCommande = Commande.StatutCommande;
                    // Insert any additional changes to column values.
                }
                context.SubmitChanges();
            }
            else
            {
                foreach (ValidationResult result in results)
                {
                    Msg += result.ErrorMessage + "\n";
                }
            }

            return Msg;
        }
        public bool DeleteCommande(Commande Commande)
        {
            
            var query = from it in context.Commandes
                        where it.Id == Commande.Id
                        select it;
            foreach (Commande cmd in query)
            {
                context.Commandes.DeleteOnSubmit(cmd);
            }
            
            context.SubmitChanges();
            return true;
        }
        public int insertNumSerie(string NumSeries, string IdEmplacement)
        {
            ConnectionPostgres connection = new ConnectionPostgres("127.0.0.1", "postgres", "admin", "CrudTest", 5432);
            connection.connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection.connection;
            string insert = "";
            char[] sparateur = new char[] { ';' };
            string[] tabNumSerie = NumSeries.Split(sparateur);
            int nb_row = 0;
            foreach (string item in tabNumSerie)
            {
                if (item != "")
                {
                    insert = "INSERT INTO \"Num_Serie_Emplacement\" (\"Id_Num_Serie\",\"Id_emplacement\") values(:Id_Num_Serie,:Id_emplacement)";
                    //La valeur DEFAULT parce que la propriété id est auto incrémenté
                    cmd = new NpgsqlCommand(insert, connection.connection);
                    cmd.Parameters.Add(new NpgsqlParameter("Id_Num_Serie", DbType.String)).Value = item;
                    cmd.Parameters.Add(new NpgsqlParameter("Id_emplacement", DbType.String)).Value = IdEmplacement;
                    nb_row += cmd.ExecuteNonQuery();
                }
                
            }
            cmd.Dispose();
            connection.connection.Close();
            return nb_row;
        }
        
    }
    class ConnectionPostgres
    {
        public NpgsqlConnection connection { get; }
        public ConnectionPostgres(string Server, string UserId, string Password, string Database, int Port)
        {

            connection = new NpgsqlConnection("Server=" + Server + ";Port=" + Port + ";User Id=" + UserId + ";Password=" + Password + ";Database=" + Database);

        }


    }
}