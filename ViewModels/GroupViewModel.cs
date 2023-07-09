using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class GroupViewModel : IEquivalence<GroupViewModel>
    {
        public int GroupId { get; set; }
        public int CourseId { get; set; }
        public CourseViewModel? Course { get; set; }
        [StringLength(30, ErrorMessage = "The maximum length for Name is 30 characters")]
        public string Name { get; set; }
        public IEnumerable<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();

        public bool IsEquivalentTo(GroupViewModel? other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return GroupId == other.GroupId &&
                   CourseId == other.CourseId &&
                   Course.IsEquivalentTo(other.Course) &&
                   Name == other.Name;
        }
    }
}
