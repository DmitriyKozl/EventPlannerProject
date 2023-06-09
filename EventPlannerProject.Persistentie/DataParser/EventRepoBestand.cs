using EventPlannerProject.Domein;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Persistentie.DataParser;

public class EventRepoBestand : IEventRepo {
    private List<Events> _events;

    public EventRepoBestand(string bestand) {
        _events = new List<Events>();
        try {
            using (var reader = new StreamReader(bestand)) {
                string line;
                string[] columnNames = reader.ReadLine().Split(',');
                while ((line = reader.ReadLine()) != null) {
                    string[] elementen = line.Split(';');

                    string[] startDateTimeParts = elementen[3].Split(' ');
                    string startDate = startDateTimeParts[0];
                    string[] startTime = startDateTimeParts[1].Split('+');
                    string[] endDateTimeParts = elementen[2].Split(' ');
                    string endDate = endDateTimeParts[0];
                    string[] endTime = endDateTimeParts[1].Split('+');
                    //// Parse the dates and times
                    DateTime startDateTime = DateTime.Parse(startDate);
                    TimeSpan startTimeSpan = TimeSpan.Parse(startTime[0]);
                    DateTime endDateTime = DateTime.Parse(endDate);
                    TimeSpan endTimeSpan = TimeSpan.Parse(endTime[0]);


                    if (string.IsNullOrEmpty(elementen[4])) {
                        elementen[4] = "0";
                    }

                    _events.Add(new Events(
                        elementen[0],
                        elementen[1],
                        startDateTime,
                        startTimeSpan,
                        endTimeSpan,
                        endDateTime,
                        elementen[5],
                        int.Parse(elementen[4])
                    ));
                }
            }
        }
        catch {
            throw;
        }
    }

    public List<Events> GetEvents() {
        return _events;
    }

    public void SaveEvenement(Events evenement) {
        throw new NotImplementedException();
    }

    public List<Events> GetEventsGebruiker(int dagPlanId) {
        throw new NotImplementedException();
    }

    public List<Events> GetEvenementenVanDatum(DateTime datum) {
        throw new NotImplementedException();
    }

    public void VoegEvenementToeAanDagplan(EventDto evenement, Dagplan dagplan) {
        throw new NotImplementedException();
    }
}