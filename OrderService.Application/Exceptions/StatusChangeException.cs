using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Exceptions
{
    public class StatusChangeException : Exception
    {
        public StatusChangeException() : base("It is not possible to change the status from the previous one to the new one")
        {
        }

        public StatusChangeException(string? message) : base(message)
        {
        }

        public StatusChangeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
