using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1
{
    public class AppContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }


        public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId);
        }

        public void SeedData()
        {
            // Проверяем, есть ли уже данные в базе
            if (!Students.Any() && !Courses.Any() && !Grades.Any() && !Teachers.Any())
            {
                // Создание учителей
                Teacher t1 = new Teacher { FirstName = "Иван", LastName = "Иванов", Age = 40, Address = "Москва" };
                Teacher t2 = new Teacher { FirstName = "Петр", LastName = "Петров", Age = 35, Address = "Санкт-Петербург" };

                Teachers.AddRange(t1, t2);
                SaveChanges(); // Сохраняем изменения, чтобы учителя получили идентификаторы

                // Создание студентов
                Student s1 = new Student { FirstName = "ОК", LastName = "Пупкин", Age = 20, Address = "Москва" };
                Student s2 = new Student { FirstName = "Да", LastName = "Иванов", Age = 25, Address = "Братск" };
                Student s3 = new Student { FirstName = "Нет", LastName = "Петров", Age = 35, Address = "Иркутск" };

                Students.AddRange(s1, s2, s3);
                SaveChanges(); // Сохраняем студентов

                // Создание курсов и связь с учителями
                Course course1 = new Course { CourseName = "Математика", TeacherId = t1.TeacherId };
                Course course2 = new Course { CourseName = "Физика", TeacherId = t2.TeacherId };

                Courses.AddRange(course1, course2);
                SaveChanges(); // Сохраняем курсы

                // Создание оценок
                Grades.AddRange(
                    new Grade { Student = s1, Course = course1, Score = 4 },
                    new Grade { Student = s2, Course = course2, Score = 3 }
                );

                SaveChanges(); // Сохраняем оценки
            }
        }
    }
}
