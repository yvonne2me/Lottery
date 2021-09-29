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
            CreateMap<Models.Domain.Line, Models.API.Line>();
        }
    }
}