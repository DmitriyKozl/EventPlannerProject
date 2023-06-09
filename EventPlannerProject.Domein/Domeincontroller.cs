using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventPlannerProject.Domein;

public class Domeincontroller
{
    private readonly IEventRepo _eventRepo;
    private readonly IGebruikerRepo _gebruikerRepo;
    private readonly IDagplanRepo _dagplanRepo;


    public Domeincontroller(IEventRepo eventRepo, IGebruikerRepo gebruikerRepo, IDagplanRepo dagplanRepo)
    {
        _eventRepo = eventRepo;
        _gebruikerRepo = gebruikerRepo;
        _dagplanRepo = dagplanRepo;
    }

    //Save methods
    public void SaveGebruiker(Gebruiker gebruiker)
    {
        _gebruikerRepo.SaveGebruiker(gebruiker);
    }
    public void SaveEvenement(Events evenement)
    {
        _eventRepo.SaveEvenement(evenement);
    }

    // base get methods

    public List<GebruikerDTO> GetGebruiker()
    {
        List<Gebruiker> gebruikers = _gebruikerRepo.GetGebruiker();
        return gebruikers.Select(g => new GebruikerDTO(g)).ToList();
    }

    public List<EventDto> GetEvents()
    {
        List<Events> events = _eventRepo.GetEvents();
        return events.Select(e => new EventDto(e)).ToList();
    }



    // get methods with parameters

    public List<Events> GetEvenementenVanDatum(DateTime datum)
    {


        return _eventRepo.GetEvenementenVanDatum(datum);

    }


    public List<Dagplan> GetDagplanGebruiker(GebruikerDTO gebruiker)
    {
        return _dagplanRepo.GetDagplanGebruiker(gebruiker);
    }

    public Dictionary<DateTime, List<DagplanDTO>> GetDagplannenPerDatum(Gebruiker gebruiker)
    {
        return _dagplanRepo.GetDagplannenPerDatum(gebruiker);
    }

    public void VoegEvenementToeAanDagplan(List<Events> evenement, Dagplan dagplan, Gebruiker gebruiker)
    {
      

        _dagplanRepo.VoegEvenementToeAanDagplan(evenement, dagplan, gebruiker);
    }

    //public DagplanDTO GetDagplanVoorDatum(DateTime datum, Gebruiker gebruiker)
    //{
    //    // Haal de dagplannen op voor de gebruiker
    //    Dictionary<DateTime, List<DagplanDTO>> dagplannenPerDatum = GetDagplannenPerDatum(gebruiker);

    //    // Controleer of er een dagplan bestaat voor de opgegeven datum
    //    if (dagplannenPerDatum.ContainsKey(datum.Date))
    //    {
    //        // Geef het eerste dagplan terug dat overeenkomt met de opgegeven datum
    //        return dagplannenPerDatum[datum.Date].FirstOrDefault();
    //    }

    //    return null; // Geen dagplan gevonden voor de opgegeven datum
    //}

    public Dagplan CreateDagplan(Gebruiker gebruiker, DateTime datum)
    {
        List<Dagplan> dagplannen = _dagplanRepo.GetDagplanGebruiker(new GebruikerDTO(gebruiker));
        bool bestaatDagplan = dagplannen.Any(dagplan => dagplan.Datum.Date == datum.Date);

        if (bestaatDagplan) {
            throw new DagplanException("Er bestaat al een dagplan voor de opgegeven datum.");
        }



        return _dagplanRepo.CreateDagplan(gebruiker, datum);
    }
}