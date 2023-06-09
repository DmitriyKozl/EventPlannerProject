using System.Data;
using System.Data.SqlClient;
using System.Linq;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Persistentie.Database;

public class DagplanDb : IDagplanRepo
{
    private string _connectionString;
    public DagplanDb(string connectionString)
    {
        _connectionString = connectionString;
    }
    public List<Dagplan> GetDagplanGebruiker(GebruikerDTO gebruiker)
    {
        List<Dagplan> dagplannen = new List<Dagplan>();
        Gebruiker user = new(gebruiker.Naam, gebruiker.Voornaam, gebruiker.GebruikerID);

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                               "Dagplan.Datum, Events.EventCode, Events.EventTitel, Events.DateTimeStart, Events.DateTimeEind, Events.StartTijd, Events.EindTijd, Events.Prijs, Events.Beschrijving, Events.EventsId " +
                               "FROM DagplanEvenement " +
                               "INNER JOIN Dagplan ON DagplanEvenement.DagplanID = Dagplan.DagplanID " +
                               "INNER JOIN Gebruiker ON Dagplan.BezoekerID = Gebruiker.GebruikerID " +
                               "INNER JOIN Events ON DagplanEvenement.EventsId = Events.EventsId " +
                               "WHERE BezoekerID = @GebruikerID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@GebruikerID", gebruiker.GebruikerID);

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string eventCode = dataReader["EventCode"].ToString();
                                string eventTitel = dataReader["EventTitel"].ToString();
                                DateTime datum = (DateTime)dataReader["Datum"];
                                DateTime datumEind = (DateTime)dataReader["DateTimeEind"];
                                TimeSpan startTijd = (TimeSpan)dataReader["StartTijd"];
                                TimeSpan eindTijd = (TimeSpan)dataReader["EindTijd"];
                                decimal prijs = (decimal)dataReader["Prijs"];
                                string beschrijving = dataReader["Beschrijving"].ToString();
                                int eventID = (int)dataReader["EventsId"];

                                Events e = new(eventCode, eventTitel, datum, startTijd, eindTijd, datumEind, beschrijving, prijs, eventID);

                                // Zoek naar het dagplan met dezelfde datum
                                Dagplan dagplan = dagplannen.FirstOrDefault(d => d.Datum == datum);

                                if (dagplan == null)
                                {
                                    // Maak een nieuw dagplan als deze nog niet bestaat
                                    dagplan = new Dagplan(datum, user);
                                    dagplannen.Add(dagplan);
                                }

                                // Voeg het evenement toe aan het juiste dagplan
                                dagplan.Event.Add(e);
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            throw;
        }

        return dagplannen;
    }
    public Dictionary<DateTime, List<DagplanDTO>> GetDagplannenPerDatum(Gebruiker gebruiker)
    {
        Dictionary<DateTime, List<Dagplan>> dagplannenPerDatum = new Dictionary<DateTime, List<Dagplan>>();

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                               "Dagplan.Datum, Events.EventCode, Events.EventTitel, Events.DateTimeStart, Events.DateTimeEind, Events.StartTijd, Events.EindTijd, Events.Prijs, Events.Beschrijving, Events.EventsId " +
                               "FROM DagplanEvenement " +
                               "INNER JOIN Dagplan ON DagplanEvenement.DagplanID = Dagplan.DagplanID " +
                               "INNER JOIN Gebruiker ON Dagplan.BezoekerID = Gebruiker.GebruikerID " +
                               "INNER JOIN Events ON DagplanEvenement.EventsId = Events.EventsId " +
                               "WHERE BezoekerID = @GebruikerID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@GebruikerID", gebruiker.GebruikerID);

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string eventCode = dataReader["EventCode"].ToString();
                            string eventTitel = dataReader["EventTitel"].ToString();
                            DateTime datum = ((DateTime)dataReader["Datum"]).Date;
                            DateTime datumEind = ((DateTime)dataReader["DateTimeEind"]).Date;
                            TimeSpan startTijd = (TimeSpan)dataReader["StartTijd"];
                            TimeSpan eindTijd = (TimeSpan)dataReader["EindTijd"];
                            decimal prijs = (decimal)dataReader["Prijs"];
                            string beschrijving = dataReader["Beschrijving"].ToString();
                            int eventID = (int)dataReader["EventsId"];

                            Events e = new Events(eventCode, eventTitel, datum, startTijd, eindTijd, datumEind, beschrijving, prijs, eventID);

                            // Controleer of de datum al bestaat in de dictionary
                            if (dagplannenPerDatum.ContainsKey(datum))
                            {
                                Dagplan dagplan = dagplannenPerDatum[datum].First();
                                dagplan.Event.Add(e);

                            }
                            else
                            {
                                // Maak een nieuwe lijst met het dagplan voor de datum
                                Dagplan dagplan = new Dagplan(datum, gebruiker);
                                dagplan.Event.Add(e);
                                dagplannenPerDatum[datum] = new List<Dagplan> { dagplan };
                            }
                        }
                    }
                }
            }
        }
        catch (DagplanException ex)
        {
            // Handel eventuele fouten af
            Console.WriteLine("Fout bij het ophalen van dagplannen: " + ex.Message);
        }

        return dagplannenPerDatum.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(dagplan => new DagplanDTO(dagplan)).ToList()
        ); ;
    }
    public void VoegDagplanToe(Dagplan dagplan)
    {
        try {
            using (SqlConnection connection = new(_connectionString)) {
                connection.Open();

                string selectSql = "SELECT COUNT(*) FROM Dagplan WHERE Datum = @Datum AND BezoekerID = @BezoekerID;";
                SqlCommand selectCommand = new SqlCommand(selectSql, connection);

                selectCommand.Parameters.Add("@Datum", SqlDbType.Date);
                selectCommand.Parameters["@Datum"].Value = dagplan.Datum;

                selectCommand.Parameters.Add("@BezoekerID", SqlDbType.Int);
                selectCommand.Parameters["@BezoekerID"].Value = dagplan.Gebruiker.GebruikerID;

                int existingCount = (int)selectCommand.ExecuteScalar();

                if (existingCount > 0) {
                    //foreach (Events e in dagplan.Event) {
                    //    string insertEventSql =
                    //        "INSERT INTO DagplanEvenement (DagplanID, EventsId, Tijdstip) VALUES (@DagplanID, @EventsId, @Tijdstip);";
                    //    SqlCommand insertEventCommand = new SqlCommand(insertEventSql, connection);

                    //    insertEventCommand.Parameters.AddWithValue("@DagplanID", dagplan.DagplanID);
                    //    insertEventCommand.Parameters.AddWithValue("@EventsId", e.EventId);
                    //    insertEventCommand.Parameters.AddWithValue("@Tijdstip", e.StartTijd);

                    //    insertEventCommand.ExecuteNonQuery();
                    //}
                    throw new DagplanException("Dit dagplan bestaat al.");

                }
                //else {
                    //Insert the Dagplan
                    string insertSql = "INSERT INTO Dagplan (Datum, BezoekerID) VALUES (@Datum, @BezoekerID);";
                    SqlCommand insertCommand = new SqlCommand(insertSql, connection);

                    insertCommand.Parameters.Add("@Datum", SqlDbType.Date);
                    insertCommand.Parameters["@Datum"].Value = dagplan.Datum;

                    insertCommand.Parameters.Add("@BezoekerID", SqlDbType.Int);
                    insertCommand.Parameters["@BezoekerID"].Value = dagplan.Gebruiker.GebruikerID;

                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                //}

            }
        }
        catch (DagplanException ex) {
            throw new DagplanException("Fout bij het toevoegen van het dagplan: " + ex.Message);
        }
    }

    public Dagplan CreateDagplan(Gebruiker gebruiker, DateTime datum)
    {
        //bool bestaatDagplan = GetDagplanGebruiker(new GebruikerDTO(gebruiker))
        //    .Any(dagplan => dagplan.Datum.Date == datum);

        //if (bestaatDagplan)
        //{
        //    throw new DagplanException("Er bestaat al een dagplan voor de geselecteerde datum.");


        //}

        //Dagplan nieuwDagplan = new Dagplan(datum, gebruiker)
        //{
        //    Datum = datum,
        //    Gebruiker = gebruiker,
        //    Event = new List<Events>()
        //};


        //VoegDagplanToe(nieuwDagplan);

        //return nieuwDagplan;
        bool bestaatDagplan = GetDagplanGebruiker(new GebruikerDTO(gebruiker))
            .Any(dagplan => dagplan.Datum.Date == datum.Date);

        if (bestaatDagplan)
        {
            throw new DagplanException("Er bestaat al een dagplan voor de geselecteerde datum.");
        }

        Dagplan nieuwDagplan = new Dagplan(datum, gebruiker)
        {
        Event = new List<Events>()
        };

        VoegDagplanToe(nieuwDagplan);

        return nieuwDagplan;

    }

    public void VoegEvenementToeAanDagplan(List<Events> evenement, Dagplan dagplan, Gebruiker gebruiker)
    {

        Dictionary<DateTime, List<DagplanDTO>> dagplannenPerDatum = GetDagplannenPerDatum(gebruiker);

        if (dagplannenPerDatum.ContainsKey(dagplan.Datum.Date))
        {
            DagplanDTO bestaandDagplan = dagplannenPerDatum[dagplan.Datum.Date].First();


            bestaandDagplan.Event.AddRange(evenement.Select(e => new EventDto(e)).ToList());


        }
        else
        {
            DagplanDTO nieuwDagplan = new DagplanDTO(dagplan);
            nieuwDagplan.Gebruiker.GebruikerID = dagplan.Gebruiker.GebruikerID;
            foreach (var e in evenement)
            {
                nieuwDagplan.Event.Add(new EventDto(e));
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query =
                    "INSERT INTO Dagplan (Datum, BezoekerID) VALUES (@Datum, @BezoekerID); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Datum", nieuwDagplan.Datum);
                    command.Parameters.AddWithValue("@BezoekerID", nieuwDagplan.Gebruiker.GebruikerID);

                    int dagplanId = Convert.ToInt32(command.ExecuteScalar());
                    foreach (var e in evenement)
                    {
                        string koppeltabelQuery =
                            "INSERT INTO DagplanEvenement (DagplanID, EventsId, Tijdstip) VALUES (@DagplanID, @EventsId, @Tijdstip);";

                        using (SqlCommand koppeltabelCommand = new SqlCommand(koppeltabelQuery, connection))
                        {
                            koppeltabelCommand.Parameters.AddWithValue("@DagplanID", dagplanId);
                            koppeltabelCommand.Parameters.AddWithValue("@EventsId", e.EventId);
                            koppeltabelCommand.Parameters.AddWithValue("@Tijdstip", e.StartTijd);

                            koppeltabelCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}


