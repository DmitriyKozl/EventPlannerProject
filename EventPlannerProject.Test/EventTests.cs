using System.Security.Cryptography.X509Certificates;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Models;
using EventPlannerProject.Domein.Exceptions;

namespace EventPlannerProject.Test
{
    public class EventTests
    {

        [Fact]
        public void ShouldSameEventBeAbleToBeAddedTwice()
        {

            Gebruiker gebruiker = new Gebruiker("Nikita", "Semjonava");

            DateTime dateTimeStart1 = new DateTime(2023, 5, 17, 9, 0, 0);
            TimeSpan timeStart1 = new TimeSpan(9, 0, 0);
            TimeSpan timeEind1 = new TimeSpan(12, 0, 0);
            DateTime dateTimeEind1 = new DateTime(2023, 5, 17, 12, 0, 0);
            Events event1 = new Events("cde432f2-8c6f-dd25-3a8d-000000000828", "Event 1", dateTimeStart1, timeStart1, timeEind1, dateTimeEind1,
                "Beschrijving 1", 10, 2);
            Dagplan dagplan = new Dagplan(new DateTime(2000, 2, 7), gebruiker);

            dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event1));

            // Act & Assert
            Assert.Throws<DagplanException>(() => dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event1)));
        }
        [Fact]
        public void EventsShouldNotOverlap()
        {
            Gebruiker gebruiker = new Gebruiker("Nikita", "Semjonava");

            DateTime dateTimeStart1 = new DateTime(2023, 5, 17, 9, 0, 0);
            TimeSpan timeStart1 = new TimeSpan(9, 0, 0);
            TimeSpan timeEind1 = new TimeSpan(12, 0, 0);
            DateTime dateTimeEind1 = new DateTime(2023, 5, 17, 12, 0, 0);
            Events event1 = new Events("cde432f2-8c6f-dd25-3a8d-000000000828", "Event 1", dateTimeStart1, timeStart1, timeEind1, dateTimeEind1,
                "Beschrijving 1", 10m);
            Dagplan dagplan = new Dagplan(new DateTime(2000, 2, 7), gebruiker);

            dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event1));

            // Act & Assert
            Assert.Throws<DagplanException>(() => dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event1)));
        }
       
        [Fact]
        public void PriceShouldNotExceedDailyBudget()
        {
            Gebruiker gebruiker = new Gebruiker("Nikita", "Semjonava");

            DateTime dateTimeStart1 = new DateTime(2023, 5, 17, 9, 0, 0);
            TimeSpan timeStart1 = new TimeSpan(9, 0, 0);
            TimeSpan timeEind1 = new TimeSpan(12, 0, 0);
            DateTime dateTimeEind1 = new DateTime(2023, 5, 17, 12, 0, 0);

            DateTime dateTimeStart2 = new DateTime(2023, 5, 16, 9, 0, 0);
            TimeSpan timeStart2 = new TimeSpan(10, 0, 0);
            TimeSpan timeEind2 = new TimeSpan(14, 0, 0);
            DateTime dateTimeEind2 = new DateTime(2023, 5, 17, 12, 0, 0);
            Events event1 = new Events("cde432f2-8c6f-dd25-3a8d-0000000828", "Event 1", dateTimeStart1, timeStart1, timeEind1, dateTimeEind1,
                "Beschrijving 1", 55m);
            Events event2 = new Events("cde43f-dd25-3a8d-000000000828", "Event 2", dateTimeStart2, timeStart2, timeEind2, dateTimeEind2,
                "Beschrijving 2", 10m);
            Dagplan dagplan = new(new(2000, 2, 7),gebruiker);

            List<Events> events = new List<Events>()
            { event1 };

            dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event2));

            // Act & Assert
            Assert.Throws<DagplanException>(() => dagplan.VoegEventToe(dagplan.GetEvents(), new EventDto(event2)));
        }
    }
}

