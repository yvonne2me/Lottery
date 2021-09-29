using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptions;
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

        public async Task<Status> GetTicketResult(Guid id)
        {
            var ticket = await this.ticketRepository.GetTicket(id);        

            if(ticket == null)
            {
                throw new TicketNotFoundException("Ticket Does Not Exist");
            }

            Status result = new Status();          
            result.LineStatus = CheckLinesOnTicket(ticket);

            await this.ticketRepository.StatusChecked(ticket);

            return result;
        }

        private List<LineStatus> CheckLinesOnTicket(Ticket ticket)
        {
            List<LineStatus> response = new List<LineStatus>();

            foreach(var line in ticket.Lines)
            {
                var lineResult = new LineStatus();
                lineResult.Numbers = line.Numbers;
                lineResult.Result = GetResult(line.Numbers);
                response.Add(lineResult);
            }

            return response.OrderByDescending(r => r.Result).ToList();
        }

        private int GetResult(string numbers)
        {
            List<int> numberList = GetListOfNumbers(numbers);

            int result = 0;

            if(numberList[0]+numberList[1]+numberList[2]==2)
            {
                result = 10;
            }
            else if(numberList[0]==numberList[1] && numberList[1]==numberList[2])
            {
                result = 5;
            }
            else if(numberList[0]!=numberList[1] && numberList[0] != numberList[2])
            {
                result = 1;
            }

            return result;
        }

        private List<int> GetListOfNumbers(string numbers)
        {
            List<int> numberList = new List<int>();

            foreach(var s in numbers.Split(',')) 
            {
                int num;
                if (int.TryParse(s, out num))
                {
                    numberList.Add(num);
                }
            }

            return numberList;
        }
    }
}