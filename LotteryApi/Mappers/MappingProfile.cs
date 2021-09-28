using AutoMapper;
using Models.API;
using Models.Domain;

namespace Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketResponse>();
            CreateMap<Line, LineResponse>();
        }
    }
}