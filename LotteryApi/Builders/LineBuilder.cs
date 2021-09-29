using System;
using System.Collections.Generic;
using Models.Domain;

namespace Builders
{
    public static class LineBuilder
    {
        public static List<Line> CreateLines(Guid ticketId, int numberOfLines)
        {
            if(numberOfLines <= 0) 
            {
                throw new ArgumentException("Invalid Number of Lines");
            }

            List<Line> lines = new List<Line>();

            for(var i=0; i < numberOfLines; i++)
            {
                var line = AddLine(ticketId);
                lines.Add(line);
            }

            return lines;
        }

        private static Line AddLine(Guid ticketId)
        {
            Line line = new Line()
            {
                Id = Guid.NewGuid(),
                TicketId = ticketId,
                Numbers = GetNumbers()
            };

            return line;
        }

        private static string GetNumbers()
        {
            Random rnd = new Random();
            int[] numbers = new int[3];

            for(var i=0; i < 3; i++)
            {
                numbers[i] = (rnd.Next(0, 3));
            }

            return string.Join(", ", numbers);
        }
    }
}