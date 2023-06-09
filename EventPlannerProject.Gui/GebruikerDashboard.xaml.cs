using EventPlannerProject.Domein.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.Exceptions;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Gui
{
    /// <summary>
    /// Interaction logic for GebruikerDashboard.xaml
    /// </summary>
    public partial class GebruikerDashboard : Window {
        public Gebruiker SelectedUser { get; set; }
        private Domeincontroller _domeincontroller;
        public List<DagplanDTO> Dagplannen { get; set; }
        public DateTime SelectedDate { get; set; }



        public GebruikerDashboard(GebruikerDTO selectedUser, Domeincontroller _domeincontroller)
        {
            InitializeComponent();
            SelectedUser = new Gebruiker(selectedUser.Naam, selectedUser.Voornaam, selectedUser.GebruikerID);
            this._domeincontroller = _domeincontroller;
            List<GebruikerDTO> gebruikers = _domeincontroller.GetGebruiker();
            Dagplannen = _domeincontroller.GetDagplanGebruiker(selectedUser).Select(dagplannen => new DagplanDTO(dagplannen)).ToList(); ;
            Dagplannen = Dagplannen.OrderBy(d => d.Datum.Date).ToList();



            gebruikerBlock.Text = "Welkom " + SelectedUser.Voornaam;

            dagplannenListBox.ItemsSource = Dagplannen;
            evenementenTextBlock.Text = string.Empty;
            totalePrijsTextBlock.Text = string.Empty;

        }

        private void DagplannenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dagplannenListBox.SelectedItem is DagplanDTO selectedDagplan)
            {
                List<EventDto> evenementen = selectedDagplan.Event;

                StringBuilder sb = new StringBuilder();

                foreach (EventDto evenement in evenementen)
                {
                    sb.AppendLine($"Eventtitel: {evenement.EventTitel}");
                    sb.AppendLine($"Starttijd: {evenement.StartTijd}");
                    sb.AppendLine($"Eindtijd: {evenement.EindTijd}");
                    sb.AppendLine();
                }

                evenementenTextBlock.Text = sb.ToString();

                decimal totalePrijs = evenementen.Sum(e => e.Prijs);
                totalePrijsTextBlock.Text = $"{totalePrijs} euro opgebruikt van {selectedDagplan.Budget} euro. Totale Resterend budget dagplan: €{selectedDagplan.Budget - totalePrijs}";

            }
        }
        private void DatumDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                SelectedDate = datePicker.SelectedDate.Value.Date;
            }
        }

        private void MaakNieuwDagplanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dagplan nieuwDagplan = _domeincontroller.CreateDagplan(SelectedUser, SelectedDate);
                SelectedUser.voegDagplantoe(Dagplannen, nieuwDagplan);
                Dagplannen.Add(new DagplanDTO(nieuwDagplan));
                EvenementenWindow evenementenToevoegenWindow = new EvenementenWindow(nieuwDagplan, _domeincontroller, new GebruikerDTO(SelectedUser));
                Close();
                evenementenToevoegenWindow.Show();
            }
            catch (DagplanException ex)
            {
                MessageBox.Show(ex.message);
            }

        }
    }
}
