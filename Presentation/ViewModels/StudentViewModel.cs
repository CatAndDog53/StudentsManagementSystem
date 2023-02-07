using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public GroupViewModel? Group { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for First Name is 30 characters")]
        public string FirstName { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for Last Name is 30 characters")]
        public string LastName { get; set; }
    }
}
