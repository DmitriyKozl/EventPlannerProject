using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerProject.Domein.Exceptions
{
    internal class GebruikerException : Exception
    {
        private string _message = "GebruikerException: ";

        public GebruikerException(string message)
        {
            _message += message;
        }
    }
}
