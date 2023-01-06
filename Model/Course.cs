namespace Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
    }
}