using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Domein.Interfaces;

public interface IEventRepo {
    List<Events> GetEvents();
    void SaveEvenement(Events evenement);
    List<Events> GetEvenementenVanDatum(DateTime datum);

   

}