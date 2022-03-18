using System.Runtime.Serialization;

namespace DBConnectivity

{
    public class LimitRegistrationException : ApplicationException
    {
        public LimitRegistrationException()
        {
        }

        public LimitRegistrationException(string? message) : base(message)
        {
        }

       
    }
}