using EventPlannerProject.Domein.Models;


namespace EventPlannerProject.Domein.DTO
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventCode { get; set; }
        public string EventTitel { get; set; }
        public DateTime DateTimeStart { get; set; }
        public TimeSpan StartTijd { get; set; }
        public DateTime DateTimeEind { get; set; }
        public TimeSpan EindTijd { get; set; }

        public decimal Prijs { get; set; }
        public string Beschrijving { get; set; }


        public EventDto(Events events)
        {
            EventId = events.EventId;
            EventTitel = events.EventTitel;
            DateTimeStart = events.DateTimeStart;
            StartTijd = events.StartTijd;
            DateTimeEind = events.DateTimeEind.Date;
            EindTijd = events.EindTijd;
            Prijs = events.Prijs;
            Beschrijving = events.Beschrijving;
        }

        public Events ToEvent() {
            return new Events(EventCode, EventTitel, DateTimeStart, StartTijd, EindTijd, DateTimeEind, Beschrijving, Prijs, EventId);
        }
        public bool Overlapt(Events e)
        {
            if (this.DateTimeStart.Date == e.DateTimeStart.Date && this.StartTijd < e.EindTijd && e.StartTijd < this.EindTijd)
            {
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
}
