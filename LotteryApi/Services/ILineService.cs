using System.Threading.Tasks;
using System.Collections.Generic;
using Models.Domain;
using System;

namespace Services
{
    public interface ILineService
    {
        List<Line> CreateLines(Guid ticketId, int numberOfLines);
        Line AddLine(Guid ticketId);
    }
}