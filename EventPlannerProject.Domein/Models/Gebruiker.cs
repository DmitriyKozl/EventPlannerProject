using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;

namespace EventPlannerProject.Domein.Models; 

public class Gebruiker {
    
    public int GebruikerID { get; set; }
    public string Naam { get; set; }

    public string Voornaam { get; set; }


    public Gebruiker(string naam, string voornaam)
    {

        Naam = naam;
        Voornaam = voornaam;

    }
    public Gebruiker(string naam, string voornaam, int gebruikerId) : this(naam, voornaam)
    {
        GebruikerID = gebruikerId;


    }

    public override string ToString() {
        return $"{Voornaam} {Naam}";
    }

    public void voegDagplantoe(List<DagplanDTO> dagplannen, Dagplan dagplan) {
        if (dagplan.DagplanBestaat(dagplannen, dagplan)) {
            throw new DagplanException("Dagplan bestaat al voor deze gebruiker");
        }
    }
    
}