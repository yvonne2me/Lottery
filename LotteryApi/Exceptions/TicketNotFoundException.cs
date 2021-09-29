using System;

namespace Exceptions
{
    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(String message) : base (message)
        {
        }
    }
}