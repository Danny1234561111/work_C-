using ConsoleApp6.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApp6
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());

            // Настройка конфигурации для загрузки из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Получение строки подключения
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // Создание экземпляра AppContext с переданной строкой подключения
            using (AppContext db = new AppContext(connectionString))
            {
                Course course1 = new Course { Title = "Iot", Duration = 120, Description = "Курс по интернету вещей" };
                Course course2 = new Course { Title = "PE", Duration = 60, Description = "Курс по физической культуре" };

                db.Add(course1);
                db.Add(course2);
                db.SaveChanges();

                Console.WriteLine("Courses added");

                var courses = db.Courses.ToList();

                foreach (var course in courses)
                {
                    Console.WriteLine($"[{course.CourseId}] {course.Title}, {course.Duration}, {course.Description}");
                }
            }
        }
    }
}
