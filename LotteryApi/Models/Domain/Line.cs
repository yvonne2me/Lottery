using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Line
    {
        public Guid Id { get; set;}
        public Guid TicketId { get; set;}
        //TODO: Come back and review this
        public string Numbers { get; set; }
    }
}