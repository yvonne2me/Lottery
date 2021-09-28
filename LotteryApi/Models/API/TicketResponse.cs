using System;
using System.Collections.Generic;

namespace Models.API
{
    public class TicketResponse
    {
        public Guid Id { get; set;}
        public List<LineResponse> Lines { get; set; }
    }
}