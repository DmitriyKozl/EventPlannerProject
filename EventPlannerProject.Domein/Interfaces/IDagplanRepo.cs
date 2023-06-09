using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Domein.Interfaces;

public interface IDagplanRepo {
    List<Dagplan> GetDagplanGebruiker(GebruikerDTO gebruiker);
    void VoegDagplanToe(Dagplan dagplan);
    Dagplan CreateDagplan(Gebruiker gebruiker, DateTime datum);
    Dictionary<DateTime, List<DagplanDTO>> GetDagplannenPerDatum(Gebruiker gebruiker);
    void VoegEvenementToeAanDagplan(List<Events> evenement, Dagplan dagplan, Gebruiker gebruiker);

}