using System.Collections.Generic;
using Task_Tracking.Models;

namespace Task_Tracking.Models
{
    public class KanbanViewModel
    {
        public List<Ticket> OpenTickets { get; set; }
        public List<Ticket> InProgressTickets { get; set; }
        public List<Ticket> ResolvedTickets { get; set; }
        public List<Ticket> BlockerTickets { get; set; }
        public List<Ticket> ClosedTickets { get; set; }
    }
}