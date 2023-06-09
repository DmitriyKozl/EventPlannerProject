using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Domein.Interfaces; 

public interface IGebruikerRepo {
    List<Gebruiker> GetGebruiker();

    void SaveGebruiker(Gebruiker gebruiker);
}