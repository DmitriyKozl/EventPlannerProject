using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Models;

namespace EventPlannerProject.Gui;

public partial class GebruikersView : Window
{

    private Domeincontroller _domeincontroller;
    private GebruikerDTO _selectedUser;

    public GebruikersView(Domeincontroller domeincontroller)
    {
        InitializeComponent();

        _domeincontroller = domeincontroller;
        List<GebruikerDTO> gebruikers = _domeincontroller.GetGebruiker();
        userListBox.ItemsSource = gebruikers;
    }

    private void UserListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Get the selected item from the ListBox
        if (userListBox.SelectedItem is GebruikerDTO selectedUser)
        {

            // Haal de geselecteerde gebruiker op
            GebruikerDTO geselecteerdeGebruiker = (GebruikerDTO)userListBox.SelectedItem;

            // Haal de dagplannen van de geselecteerde gebruiker op

            List<Dagplan> dagplannen = _domeincontroller.GetDagplanGebruiker(geselecteerdeGebruiker);
            GebruikerDashboard GbD = new GebruikerDashboard(geselecteerdeGebruiker, _domeincontroller);
          GbD.Show();
            Close();
        }
    }
    private void NieuwDagplan_Click(object sender, RoutedEventArgs e)
    {

        if (userListBox.SelectedItem is GebruikerDTO selectedUser)
        {

            // Haal de geselecteerde gebruiker op
            GebruikerDTO geselecteerdeGebruiker = (GebruikerDTO)userListBox.SelectedItem;

            // Haal de dagplannen van de geselecteerde gebruiker op

            List<Dagplan> dagplannen = _domeincontroller.GetDagplanGebruiker(geselecteerdeGebruiker);

            GebruikerDashboard GbD = new GebruikerDashboard(geselecteerdeGebruiker, _domeincontroller);
            GbD.Show();
            Close();
        }


    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {

        string searchQuery = txtSearch.Text.Trim();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            // Zoek de gebruikers op basis van de zoekopdracht via de domeincontroller
            List<GebruikerDTO> zoek = _domeincontroller.GetGebruiker()
                .FindAll(gebruiker => gebruiker.Naam.ToLower().Contains(searchQuery.ToLower()) ||
                                      gebruiker.Voornaam.ToLower().Contains(searchQuery.ToLower()));

            // Toon de zoekresultaten in de ListView
            userListBox.ItemsSource = zoek;
        }
        else
        {
            // Leeg de ListView als de zoekopdracht leeg is
            userListBox.ItemsSource = _domeincontroller.GetGebruiker();
            //txtSelectedUser.Text = null;

        }
    }
}
