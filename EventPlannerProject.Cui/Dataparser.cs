using EventPlannerProject.Persistentie.DataParser;
using System.ComponentModel.Design;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;
using EventPlannerProject.Persistentie.Database;

namespace EventPlannerProject.Cui {
    internal class Dataparser {
        static void Main(string[] args) {

            //Events Opslaan
            IEventRepo eventRepoBestand =
                new EventRepoBestand(
                    @"C:\CursusPB\EindOpdracht_programmeren\Data\gentse-feesten-evenementen-202223 - Sem2.csv");
            IEventRepo EventRepo =
                new EventDb(
                    @"Data Source=.\SQLEXPRESS;Initial Catalog=EventDb;Integrated Security=True;TrustServerCertificate=True");
            List<Events> events = eventRepoBestand.GetEvents();
            foreach (Events e in events) {
                EventRepo.SaveEvenement(e);
                Console.WriteLine(e);
            }
            //Gebruikers Opslaan
            IGebruikerRepo gebruikerRepoBestand = new GebruikerRepoBestand(
                    @"C:\Users\Dmitriy\Desktop\EindOpdrachtGentseFeesten-tweedekans\EindOpdrachtGentseFeesten-master\Data\gebruikerData.csv");
                IGebruikerRepo gebruikerRepo =
                    new GebruikerDb(
                        @"Data Source=.\SQLEXPRESS;Initial Catalog=EventDb;Integrated Security=True;TrustServerCertificate=True");
                List<Gebruiker> gebruikers = gebruikerRepoBestand.GetGebruiker();
                foreach (Gebruiker g in gebruikers) {
                    gebruikerRepo.SaveGebruiker(g);
                    Console.WriteLine(g);
                }





            
        }
    }
}