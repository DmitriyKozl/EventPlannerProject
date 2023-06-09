using EventPlannerProject.Domein.DTO;
using EventPlannerProject.Domein.Exceptions;
using EventPlannerProject.Domein.Interfaces;
using EventPlannerProject.Domein.Models;
using EventPlannerProject.Persistentie.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerProject.Test
{
    public class DagplanTests {



        [Fact]
        public void VoegDagplanToe_ThrowsDagplanException_WhenDagplanExistsForSelectedDatum() {
            // Arrange
            Gebruiker gebruiker = new("Semjonavo", "Nikita", 1);
            var dagplannen = new List<DagplanDTO>
            { new DagplanDTO(new Dagplan(DateTime.Parse("2023-01-01"), gebruiker)) };


            var dagplan = new Dagplan(DateTime.Parse("2023-01-01"), gebruiker);

            // Act and Assert
            Assert.Throws<DagplanException>(() => gebruiker.voegDagplantoe(dagplannen, dagplan));

        }
    }
}




