using EventPlannerProject.Domein.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerProject.Domein.DTO
{
    public class DagplanDTO
    {
        public int DagplanID { get; set; }
        public DateTime Datum { get; set; }
        public Gebruiker Gebruiker { get; set; }

        public decimal Budget { get; set; }

        public List<EventDto> Event { get; set; }
        public decimal TotaalPrijs { get; set; }


        public DagplanDTO(Dagplan dagplan) {
            DagplanID = dagplan.DagplanID;
            Datum = dagplan.Datum;
            Gebruiker = dagplan.Gebruiker;
            Event = dagplan.GetEvents().Select(e => new EventDto(e)).ToList();
            Budget = dagplan.Budget;
            TotaalPrijs = dagplan.TotaalPrijs;
        }

        public List<EventDto> GetEvents() {

            return Event;
        }
   
    }

}
