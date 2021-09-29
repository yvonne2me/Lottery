using System.ComponentModel.DataAnnotations;

namespace Models.API
{
    public class TicketRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int NumberOfLines { get; set; }
    }
}