using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EventPlannerProject.Domein;
using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;
using EventPlannerProject.Domein.Models;
using EventPlannerProject.Persistentie.Database;

namespace EventPlannerProject.Gui;

public partial class EvenementenWindow : Window
{
    public DateTime GeselecteerdeDatum { get; set; }
    public Domeincontroller _dc { get; set; }
    public Dagplan NieuwDagplan { get; set; }
    public Gebruiker GeselecteerdeGebruiker { get; set; }




    public EvenementenWindow(Dagplan nieuwDagplan, Domeincontroller domeincontroller,
        GebruikerDTO geselecteerdeGebruiker)
    {
        InitializeComponent();
        _dc = domeincontroller;

        GeselecteerdeGebruiker = new(geselecteerdeGebruiker.Naam, geselecteerdeGebruiker.Voornaam,
            geselecteerdeGebruiker.GebruikerID);
        GeselecteerdeDatum = nieuwDagplan.Datum;
        evenementenListView.ItemsSource =
            _dc.GetEvenementenVanDatum(GeselecteerdeDatum)
                .OrderBy(events => events.StartTijd)
                .Select(events => new EventDto(events)).ToList();

        NieuwDagplan = nieuwDagplan;
        DatumText.Text = $"Datum dagplan {GeselecteerdeDatum.Date.ToString("dd/MM/yyyy")}";



    }
    private void evenementenListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EventDto selectedEvent = evenementenListView.SelectedItem as EventDto;

        try
        {
            if (selectedEvent != null)
            {

                List<EventDto> geselecteerdeEvenementenDto =
                    geselecteerdeEvenementenListView.Items.Cast<EventDto>().ToList();
                List<Events> geselecteerdeEvenementen =
                    geselecteerdeEvenementenDto.Select(dto => dto.ToEvent()).ToList();

                NieuwDagplan.VoegEventToe(geselecteerdeEvenementen, selectedEvent);
                geselecteerdeEvenementenListView.Items.Add(selectedEvent);
                NieuwDagplan.TotaalPrijs = geselecteerdeEvenementenListView.Items
                    .Cast<EventDto>()
                    .Sum(item => item.Prijs);
                totalPrijsTextBox.Text = $"De dagprijs {NieuwDagplan.TotaalPrijs}";
            }
        }
        catch (DagplanException ex)
        {
            MessageBox.Show(ex.message);
        }
    }
    private void geselecteerdeEvenementenListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {


        EventDto selectedEvent = geselecteerdeEvenementenListView.SelectedItem as EventDto;
        if (selectedEvent != null)
        {
            decimal totalPriceBeforeRemoval = NieuwDagplan.TotaalPrijs;
            geselecteerdeEvenementenListView.Items.Remove(selectedEvent);
            decimal totalPriceAfterRemoval = totalPriceBeforeRemoval - selectedEvent.Prijs;
            NieuwDagplan.TotaalPrijs = Math.Max(totalPriceAfterRemoval, 0);
            totalPrijsTextBox.Text = $"De dagprijs {NieuwDagplan.TotaalPrijs}";
        }




    }
    private void ToevoegenButton_Click(object sender, RoutedEventArgs e)
    {
        List<EventDto> geselecteerdeEvenementenDto =
            geselecteerdeEvenementenListView.Items.Cast<EventDto>().ToList();

        List<Events> geselecteerdeEvenementen = geselecteerdeEvenementenDto.Select(dto => dto.ToEvent()).ToList();
        _dc.VoegEvenementToeAanDagplan(geselecteerdeEvenementen, NieuwDagplan, GeselecteerdeGebruiker);

        MessageBox.Show("Evenement(en) toegevoegd aan dagplan.");
        GebruikerDashboard GbD = new GebruikerDashboard(new GebruikerDTO(GeselecteerdeGebruiker), _dc);
        GbD.Show();
        Close();


    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {

        string searchQuery = txtSearch.Text.Trim();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            List<EventDto> zoek = _dc.GetEvenementenVanDatum(GeselecteerdeDatum).OrderBy(events => events.StartTijd)
                    .Select(events => new EventDto(events)).ToList()
                .FindAll(events => events.EventTitel.ToLower().Contains(searchQuery.ToLower()));
            evenementenListView.ItemsSource = zoek;
        }
        else
        {
            evenementenListView.ItemsSource = _dc.GetEvenementenVanDatum(GeselecteerdeDatum)
                .OrderBy(events => events.StartTijd)
                .Select(events => new EventDto(events)).ToList();

            //txtSelectedUser.Text = null;

        }
    }

}
