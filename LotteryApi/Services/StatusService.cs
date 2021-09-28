using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.API;
using Models.Domain;
using Repositories;

namespace Services
{
    public class StatusService : IStatusService
    {
        ITicketRepository ticketRepository;
        public StatusService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public async Task<StatusResponse> GetTicketStatus(Guid id)
        {
            var ticket = await this.ticketRepository.GetTicket(id);

            StatusResponse statusResponse = new StatusResponse();          
            statusResponse.StatusLineResponses = CheckLinesOnTicket(ticket);

            return statusResponse;

            //UPDATE Ticket - Checked = TRUE
        }

        private List<StatusLineResponse> CheckLinesOnTicket(Ticket ticket)
        {
            List<StatusLineResponse> response = new List<StatusLineResponse>();

            foreach(var line in ticket.Lines)
            {
                var statusLineResponse = new StatusLineResponse();
                statusLineResponse.Numbers = line.Numbers;
                statusLineResponse.Result = GetResult(line.Numbers);
                response.Add(statusLineResponse);
            }

            return response.OrderBy(r => r.Result).ToList();
        }

        private int GetResult(string numbers)
        {
            List<int> orderedList = new List<int>();

            foreach(var s in numbers.Split(',')) 
            {
                int num;
                if (int.TryParse(s, out num))
                {
                    orderedList.Add(num);
                }
            }

            int result = 0;

            if(orderedList[0]+orderedList[1]+orderedList[2]==2)
            {
                result = 10;
            }
            else if(orderedList[0]==orderedList[1] && orderedList[1]==orderedList[2])
            {
                result = 5;
            }
            else if(orderedList[0]!=orderedList[1] && orderedList[0] != orderedList[2])
            {
                result = 1;
            }

            return result;
        } 
    }
}