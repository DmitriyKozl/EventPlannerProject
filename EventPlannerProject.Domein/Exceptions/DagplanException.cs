using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerProject.Domein.Exceptions
{
    public class DagplanException : Exception
    {
        public string message = "DagplanExceptie: ";

        public DagplanException(string message)
        {
            this.message += message;
        }
    }
}
