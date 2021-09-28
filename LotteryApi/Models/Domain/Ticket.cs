using System;
using System.Collections.Generic;

namespace Models.Domain
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public List<Line> Lines { get; set; }
        private bool Checked { get; set; }
    }
}