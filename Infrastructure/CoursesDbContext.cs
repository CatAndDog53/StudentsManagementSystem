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

        private void BuildCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(action =>
            {
                action.Property(course => course.Name).HasMaxLength(30);
                action.Property(course => course.Description).HasMaxLength(500);

                action.HasMany(course => course.Groups);
            });
        }

        private void BuildGroups(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(action =>
            {
                action.Property(group => group.Name).HasMaxLength(30);

                action.HasOne(group => group.Course).WithMany(course => course.Groups)
                    .HasForeignKey(group => group.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                action.Navigation(group => group.Course).AutoInclude();
            });
        }

        private void BuildStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(action =>
            {
                action.Property(student => student.FirstName).HasMaxLength(30);
                action.Property(student => student.LastName).HasMaxLength(30);

                action.HasOne(student => student.Group).WithMany(group => group.Students)
                    .HasForeignKey(student => student.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                action.Navigation(student => student.Group).AutoInclude();
            });
        }
    }
}
