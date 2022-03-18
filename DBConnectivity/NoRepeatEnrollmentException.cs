using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectivity
{
    public class NoRepeatEnrollmentException : ApplicationException
    {
        public NoRepeatEnrollmentException() { }
        public NoRepeatEnrollmentException(string message) : base(message)
        {
                
        }
    }
}
