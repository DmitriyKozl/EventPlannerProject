using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;

namespace EventPlannerProject.Domein.Models;

public class Dagplan
{
    public int DagplanID { get; set; }
    public DateTime Datum { get; set; }
    public Gebruiker Gebruiker { get; set; }
    public decimal Budget { get; set; }
    public List<Events> Event { get; set; }


    private decimal _totaalPrijs;

    public decimal TotaalPrijs
    {
        get { return _totaalPrijs; }
        set
        {
            // Check if the value is within the allowed range
            if (value <= Budget)
            {
                _totaalPrijs = value;
            }
            else
            {
                throw new DagplanException("De totale prijs mag niet hoger zijn dan 64 euro.");
            }
        }
    }
    //public List<Events> Events { get; set; }
    public Dagplan(DateTime datum, Gebruiker gebruiker)
    {
        Datum = datum;
        Gebruiker = gebruiker;
        Event = new();
        Budget = 60m;

        TotaalPrijs = Event.Sum(item => item.Prijs);


    }

    public Dagplan(DateTime datum, Gebruiker gebruiker, decimal budget) : this(datum, gebruiker)
    {
        Budget = budget;
    }

    public List<Events> GetEvents()
    {
        return Event;
    }
    public Dagplan ToDagplan(DagplanDTO dagplanDto)
    {
        Dagplan dagplan = new Dagplan(dagplanDto.Datum, Gebruiker);
        dagplan.DagplanID = dagplanDto.DagplanID;
        // Convert the list of EventsDto to a list of Events
        dagplan.Event = dagplanDto.GetEvents().Select(eventDto =>
            eventDto.ToEvent()
        ).ToList();

        return dagplan;

    }

    public void VoegEventToe(List<Events> events, EventDto selectedEvent)
    {
        decimal TotaalPrijs
            = events.Sum(evenement => evenement.Prijs);


        // Update the TextBox with the sum

        if (TotaalPrijs + selectedEvent.Prijs > Budget)
        {
            throw new DagplanException($"De totale prijs mag niet meer dan {Budget} euro zijn.");
        }
        if (events.Any(existingEvent => existingEvent.Same(selectedEvent.ToEvent())))
        {
            throw new DagplanException("Je probeert hetzelfde evenement toe te voegen");
        }
        else if (events.Any(existingEvent => existingEvent.Overlapt(selectedEvent.ToEvent())))
        {
            throw new DagplanException("Er is overlap met een geselecteerd evenement");
        }

        else if (events.Any(existingEvent => existingEvent.OverlaptMinderDan30Minuten(selectedEvent.ToEvent())))
        {
            throw new DagplanException("Er is minder dan 30 minuten marge tussen de geselecteerde evenementen");
        }
        events.Add(selectedEvent.ToEvent()); // Add the selected event to the list

    }

    public bool DagplanBestaat(List<DagplanDTO> dagplannen, Dagplan dagplan) {
        bool bestaatDagplan = dagplannen.Any(d => d.Datum.Date == dagplan.Datum.Date);

        if (bestaatDagplan)
        {
            return true;
        }

        return false;

    }
}