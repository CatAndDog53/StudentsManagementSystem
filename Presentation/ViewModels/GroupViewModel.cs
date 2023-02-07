using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public int CourseId { get; set; }
        public CourseViewModel? Course { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for Name is 30 characters")]
        public string Name { get; set; }
        public IEnumerable<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();
    }
}
