namespace EventPlannerProject.Domein.Models; 

public class Events {
    public int EventId { get; set; }
    public string EventCode { get; set; }
    public string EventTitel { get; set; }
    public DateTime DateTimeStart { get; set; }
    public TimeSpan StartTijd { get; set; }
    public DateTime DateTimeEind { get; set; }
    public TimeSpan EindTijd { get; set; }

    public decimal Prijs { get; set; }
    public string Beschrijving   { get; set; }   
    
    public Events(string eventCode, string eventTitel, DateTime dateTimeStart, TimeSpan timeStart, TimeSpan timeEind,DateTime dateTimeEind, string beschrijving, decimal prijs) {
        EventTitel   = eventTitel;
        DateTimeStart = dateTimeStart.Date;
        StartTijd = timeStart;
        DateTimeEind = dateTimeEind.Date;
        EindTijd = timeEind;
        Prijs = prijs;
        Beschrijving = beschrijving;
        EventCode = eventCode;

    }

    public Events(string eventCode, string eventTitel, DateTime dateTimeStart, TimeSpan timeStart, TimeSpan timeEind, DateTime dateTimeEind ,string beschrijving, decimal prijs, int eventId) : this(eventCode,
        eventTitel, dateTimeStart, timeStart, timeEind,dateTimeEind, beschrijving, prijs) {
        EventId = eventId;
    }

    public override string ToString() {
        return $"{EventTitel} {DateTimeStart} {StartTijd} {DateTimeEind} {EindTijd} {Prijs} {Beschrijving}";
    }

    public bool Overlapt(Events e)
    {
        if (this.DateTimeStart.Date == e.DateTimeStart.Date && this.StartTijd <= e.EindTijd && e.StartTijd <= this.EindTijd)
        {
            return true;
        }
        else return false;
    }

    public bool Same(Events e) {
        if (this.EventId == e.EventId) {
            return true;
        }
        else return false;
    }

    public bool OverlaptMinderDan30Minuten(Events e)
    {
        TimeSpan thirtyMinutes = TimeSpan.FromMinutes(30);

        if (this.DateTimeStart.Date == e.DateTimeStart.Date && this.StartTijd < e.StartTijd.Add(thirtyMinutes) && e.StartTijd < this.EindTijd.Add(thirtyMinutes))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}