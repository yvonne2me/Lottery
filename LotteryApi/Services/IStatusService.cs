using System;
using System.Threading.Tasks;
using Models.API;

namespace Services
{
    public interface IStatusService
    {
        Task<Status> GetTicketResult(Guid id);
    }
}