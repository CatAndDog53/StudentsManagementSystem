using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public GroupViewModel? Group { get; set; }
        [Display(Name = "First Name")]
        [RegularExpression(@"[A-Za-z-А-Яа-яёЁЇїІіЄєҐґ]+", ErrorMessage = "The First Name can contain only letters")]
        [StringLength(30, ErrorMessage = "The maximum length for First Name is 30 characters")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [RegularExpression(@"[A-Za-z-А-Яа-яёЁЇїІіЄєҐґ]+", ErrorMessage = "The Last Name can contain only letters")]
        [StringLength(30, ErrorMessage = "The maximum length for Last Name is 30 characters")]
        public string LastName { get; set; }
    }
}
