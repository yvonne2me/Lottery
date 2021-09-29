using System;
using System.Collections.Generic;

namespace Models.Domain
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public List<Line> Lines { get; set; }
        public bool Checked { get; set; }
    }
}