using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Persistentie.Database;

namespace EventPlannerProject.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)

        {
            String connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=EventDb;Integrated Security=True;TrustServerCertificate=True";


            IEventRepo eventRepoDb = new EventDb(connectionString);
            IGebruikerRepo gebruikerRepoDb = new GebruikerDb(connectionString);
            IDagplanRepo dagplanRepoDb = new DagplanDb(connectionString);


            Domeincontroller dc = new Domeincontroller(eventRepoDb, gebruikerRepoDb, dagplanRepoDb);

            // Create the startup window
            GebruikersView wnd = new GebruikersView(dc);

            // Show the window
            wnd.Show();
        }
    }
}