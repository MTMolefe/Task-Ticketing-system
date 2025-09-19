using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task_Tracking.Models
{
    
    
        public class Ticket
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Title { get; set; }

            [Required]
            [StringLength(2000)]
            public string Description { get; set; }

            public TicketStatus Status { get; set; }

            [Required]
            public string Priority { get; set; }

            public DateTime CreationDate { get; set; }

            public DateTime DueDate { get; set; }

            [ForeignKey("AssignedToUser")]
            public int? AssignedToUserId { get; set; }
            public User AssignedToUser { get; set; }

            [Required]
            [ForeignKey("CreatorUser")]
            public int CreatorUserId { get; set; }
            public User CreatorUser { get; set; }
        }
    

    public enum TicketStatus
    {
        Open,
        InProgress,
        Resolved,
        Blocker,
        Closed
    }
}