using System;

namespace Models.Domain
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public int NumberOfLines { get; set; }
        private bool Checked { get; set; }
    }
}