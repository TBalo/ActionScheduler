using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODO_LIST.Models
{
    public class TodoList
    {
        [Key]
        public int ListId { get; set; }

        [Required(ErrorMessage = "Task description is required.")]
        [StringLength(500, ErrorMessage = "Task description cannot exceed 500 characters.")]
        public string Task { get; set; } = string.Empty;

        [Required]
        public bool Status { get; set; } = false;

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
