using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Infrastructure
{
    public class CoursesDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }

        public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildCourses(modelBuilder);
            BuildGroups(modelBuilder);
            BuildStudents(modelBuilder);
        }

        private void BuildStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(action =>
            {
                action.Property(student => student.StudentId)
                    .IsRequired();
            });
        }

        private void BuildGroups(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(action =>
            {
                action.Property(group => group.GroupId)
                    .IsRequired();
            });
        }

        private void BuildCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(action =>
            {
                action.Property(course => course.CourseId)
                    .IsRequired();

                action.HasMany(course => course.Groups);
            });
        }
    }
}
