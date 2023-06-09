using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerProject.Domein.Exceptions
{
    internal class EventException : Exception
    {
        private string _message = "EventException: ";

        public EventException(string message)
        {
            _message += message;
        }
    }
}
