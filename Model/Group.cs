using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Group
    {
        public int GroupId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
    }
}
