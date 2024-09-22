using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODO_LIST.Models
{
    public class TodoList
    {
        [Key]
        public int ListId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string TaskDescription { get; set; }
        
        public bool Status { get; set; }
        
        public DateTime DueDate { get; set; }

        // Foreign key for User
        public int UserId { get; set; }

        // Mark as virtual to avoid it being included in the payload
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
