using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ViewModels
{
    public class StudentViewModel : IEquivalence<StudentViewModel>
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

        public bool IsEquivalentTo(StudentViewModel? other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return StudentId == other.StudentId &&
                   GroupId == other.GroupId &&
                   Group.IsEquivalentTo(other.Group) &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName;
        }
    }
}
