using System.Data;
using System.Data.SqlClient;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Persistentie.Database; 

public class GebruikerDb : IGebruikerRepo {
    private string _connectionString;

    public GebruikerDb(string connectionString) {
        _connectionString = connectionString;
    }
    public List<Gebruiker> GetGebruiker() {
        List<Gebruiker> _gebruiker = new List<Gebruiker>();
        try {
            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();

                string query = "SELECT GebruikerId, Naam, Voornaam FROM Gebruiker";

                using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@GebruikerID", 1);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                int gebruikerId = Convert.ToInt32(reader["GebruikerID"]);
                                string naam = reader["Naam"].ToString();
                                string voornaam = reader["Voornaam"].ToString();

                                Gebruiker gebruiker = new Gebruiker(naam, voornaam);
                                gebruiker.GebruikerID = gebruikerId;

                                _gebruiker.Add(gebruiker);
                            }
                        }
                        else { }
                    }
                }
            }


        }
        catch {
            throw;
        }

        return _gebruiker; // Gebruiker niet gevonden

    }
    public void SaveGebruiker(Gebruiker gebruiker)
    {
        //if (!checkEmptyTable()) {

        //    try {
        //        string query = "TRUNCATE TABLE Gebruiker";

        //        using (SqlConnection connection = new(_connectionString)) {
        //            using (SqlCommand command = new SqlCommand(query, connection)) {
        //                connection.Open();
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e) {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        try
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                string insertSql = $"INSERT INTO Gebruiker (Naam, Voornaam)" +
                                   $"VALUES (@Naam, @Voornaam);";
                SqlCommand cmd = new(insertSql, connection);

                cmd.Parameters.Add("@Naam", SqlDbType.VarChar);
                cmd.Parameters["@Naam"].Value = gebruiker.Naam;

                cmd.Parameters.Add("@Voornaam", SqlDbType.VarChar);
                cmd.Parameters["@Voornaam"].Value = gebruiker.Voornaam;

                cmd.ExecuteNonQuery();

            }
        }
        catch
        {
            throw;
        }
    }

}