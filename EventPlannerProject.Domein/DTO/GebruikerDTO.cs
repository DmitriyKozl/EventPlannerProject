using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Domein.DTO
{
    public class GebruikerDTO
    {
        public int GebruikerID { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }

        public GebruikerDTO(Gebruiker gebruiker) {
            GebruikerID = gebruiker.GebruikerID;
            Voornaam = gebruiker.Voornaam;
            Naam = gebruiker.Naam;
        }
    }
}
