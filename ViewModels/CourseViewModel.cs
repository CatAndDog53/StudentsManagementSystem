using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for Name is 30 characters")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "The maximum length for Description is 500 characters")]
        public string Description { get; set; }
        public IEnumerable<GroupViewModel> Groups { get; set; } = new List<GroupViewModel>();
    }
}
