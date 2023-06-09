using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Persistentie.DataParser;

public class GebruikerRepoBestand : IGebruikerRepo {
    private List<Gebruiker> _gebruikers;

    public GebruikerRepoBestand(string bestand) {
        try {
            using (var reader = new StreamReader(bestand)) {
                string line;
                int index = 0;
                _gebruikers = new List<Gebruiker>();

                while ((line = reader.ReadLine()) != null) {
                    string[] elementen = line.Split(';');
                    _gebruikers.Add(new Gebruiker(
                        elementen[0],
                        elementen[1]
                    ));
                }
            }
        }
        catch {
            throw;
        }
    }

    public List<Gebruiker> GetGebruiker() {
        return _gebruikers;
    }
    public void SaveGebruiker() {
        throw new NotImplementedException();
    }

    void IGebruikerRepo.SaveGebruiker(Gebruiker gebruiker)
    {
        throw new NotImplementedException();
    }
}