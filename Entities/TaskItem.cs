using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title can't be Empty")]
        [MaxLength(20, ErrorMessage = "Character limit is 20")]
        [RegularExpression(@"^[A-Za-z\s.]+$", ErrorMessage = "The Title field can only contain alphabets, spaces, and dots.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Add short description of the Task")]
        [MaxLength(50, ErrorMessage = "Character limit is 50")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
