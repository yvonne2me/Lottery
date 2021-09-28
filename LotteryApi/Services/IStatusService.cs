using System;
using System.Threading.Tasks;
using Models.API;

namespace Services
{
    public interface IStatusService
    {
        Task<StatusResponse> GetTicketStatus(Guid id);
    }
}