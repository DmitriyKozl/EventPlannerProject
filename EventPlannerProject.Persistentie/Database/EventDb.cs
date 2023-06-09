using System.Data;
using System.Data.SqlClient;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Persistentie.Database;

public class EventDb : IEventRepo
{
    private string _connectionString;

    public EventDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Events> GetEvents()
    {
        List<Events> _events = new List<Events>();
        try
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                SqlCommand command = new("SELECT " +
                                         "EventCode, DateTimeStart, StartTijd, DateTimeEind,EindTijd, EventTitel, Prijs, Beschrijving" +
                                         " FROM Events;", connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            // Type omzetten
                            _events.Add(new Events(
                                (string)dataReader["EventCode"],
                                (string)dataReader["EventTitel"],
                                (DateTime)dataReader["DateTimeStart"],
                                (TimeSpan)dataReader["StartTijd"],
                                (TimeSpan)dataReader["EindTijd"],
                                (DateTime)dataReader["DateTimeEind"],
                                (string)dataReader["Beschrijving"],
                                (int)dataReader["Prijs"]
                            ));
                        }
                    }
                    else { }
                }
            }
        }
        catch
        {
            throw;
        }

        return _events;
    }

    public void SaveEvenement(Events events)
    {
        try
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                string insertSql =
                    $"INSERT INTO Events (EventCode, DateTimeStart,StartTijd, DateTimeEind,EindTijd, EventTitel, Prijs, Beschrijving) " +
                    $"VALUES (@EventCode, @DateTimeStart,@StartTijd, @DateTimeEind,@EindTijd, @EventTitel, @Prijs, @Beschrijving);";
                SqlCommand cmd = new(insertSql, connection);

                cmd.Parameters.Add("@EventCode", SqlDbType.VarChar);
                cmd.Parameters["@EventCode"].Value = events.EventCode;

                cmd.Parameters.Add("@DateTimeStart", SqlDbType.DateTime);
                cmd.Parameters["@DateTimeStart"].Value = events.DateTimeStart;

                cmd.Parameters.Add("@StartTijd", SqlDbType.Time);
                cmd.Parameters["@StartTijd"].Value = events.StartTijd;

                cmd.Parameters.Add("@DateTimeEind", SqlDbType.DateTime);
                cmd.Parameters["@DateTimeEind"].Value = events.DateTimeEind;

                cmd.Parameters.Add("@EindTijd", SqlDbType.Time);
                cmd.Parameters["@EindTijd"].Value = events.EindTijd;

                cmd.Parameters.Add("@EventTitel", SqlDbType.VarChar);
                cmd.Parameters["@EventTitel"].Value = events.EventTitel;

                cmd.Parameters.Add("@Prijs", SqlDbType.Int);
                cmd.Parameters["@Prijs"].Value = events.Prijs;


                if (string.IsNullOrWhiteSpace(events.Beschrijving))
                    cmd.Parameters.AddWithValue("@Beschrijving", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Beschrijving", events.Beschrijving);

                // wat is dit? 
                // voert heel de query uit.
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            throw;
        }
    }

    public List<Events> GetEvenementenVanDatum(DateTime datum)
    {
        List<Events> evenementen = new List<Events>();

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Events WHERE DateTimeStart = @DateTimeStart";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DateTimeStart", datum.Date);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string eventCode = reader["EventCode"].ToString();
                            string eventTitel = reader["EventTitel"].ToString();
                            DateTime dateTimeStart = (DateTime)reader["DateTimeStart"];
                            DateTime dateTimeEind = (DateTime)reader["DateTimeEind"];
                            TimeSpan startTijd = (TimeSpan)reader["StartTijd"];
                            TimeSpan eindTijd = (TimeSpan)reader["EindTijd"];
                            decimal prijs = (decimal)reader["Prijs"];
                            string beschrijving = reader["Beschrijving"].ToString();
                            int eventID = (int)reader["EventsId"];

                            Events e = new(eventCode,eventTitel, dateTimeStart, startTijd, eindTijd, dateTimeEind, beschrijving, prijs, eventID);

                            evenementen.Add(e);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van evenementen: " + ex.Message);
        }

        return evenementen;
            //.Select(ev => new EventDto(ev)).ToList();
    }

}