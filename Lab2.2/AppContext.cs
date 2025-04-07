using ConsoleApp6.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    public class AppContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Grade> Grades { get; set; } = null!;

        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;


        public AppContext() { Database.EnsureDeleted(); Database.EnsureCreated(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=sqlite20250404.db");
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            //optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);

            // Включение ленивой загрузки
            optionsBuilder.UseLazyLoadingProxies().UseSqlite(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка составного ключа для Enrollment
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

            // Настройка связи между Course и Teacher
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher) // Указываем, что у Course есть один Teacher
                .WithMany() // Указываем, что у Teacher может быть много Courses
                .HasForeignKey(c => c.TeacherId); // Указываем, что TeacherID является внешним ключом
        }


        public void SeedData()
        {
            if (!Students.Any() && !Courses.Any() && !Grades.Any())
            {
                // Создание учителей
                Teacher t1 = new Teacher { FirstName = "Иван", LastName = "Иванов", Age = 40, Address = "Москва" };
                Teacher t2 = new Teacher { FirstName = "Петр", LastName = "Петров", Age = 35, Address = "Санкт-Петербург" };

                // Добавление учителей в контекст
                Teachers.Add(t1);
                Teachers.Add(t2);
                SaveChanges(); // Сохраняем изменения, чтобы учителя получили идентификаторы

                // Создание студентов
                Student s1 = new Student { FirstName = "ОК", LastName = "Пупкин", Age = 20, Address = "Москва" };
                Student s2 = new Student { FirstName = "Да", LastName = "Иванов", Age = 25, Address = "Братск" };
                Student s3 = new Student { FirstName = "Нет", LastName = "Петров", Age = 35, Address = "Иркутск" };

                Students.Add(s1);
                Students.Add(s2);
                Students.Add(s3);

                // Создание курсов и связь с учителями
                Course course1 = new Course { CourseName = "Математика", TeacherId = t1.TeacherId };
                Course course2 = new Course { CourseName = "Физика", TeacherId = t2.TeacherId };

                Courses.Add(course1);
                Courses.Add(course2);

                // Создание оценок
                Grades.Add(new Grade { Student = s1, Course = course1, Score = 4 });
                Grades.Add(new Grade { Student = s2, Course = course2, Score = 3 });

                SaveChanges();
            }
        }


        public void UpdateStudent(int studentId, string newFirstName, string newLastName, int newAge, string newAddress)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                student.FirstName = newFirstName;
                student.LastName = newLastName;
                student.Age = newAge;
                student.Address = newAddress;
                SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                Students.Remove(student);
                SaveChanges();
            }
        }
    }
}
