using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ViewModels
{
    public class CourseViewModel : IEquivalence<CourseViewModel>
    {
        public int CourseId { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for Name is 30 characters")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "The maximum length for Description is 500 characters")]
        public string Description { get; set; }
        public IEnumerable<GroupViewModel> Groups { get; set; } = new List<GroupViewModel>();

        public bool IsEquivalentTo(CourseViewModel? other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return CourseId == other.CourseId &&
                   Name == other.Name &&
                   Description == other.Description;
        }
    }
}
