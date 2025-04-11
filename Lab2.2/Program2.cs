using ConsoleApp6.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ConsoleApp6
{
    class Program
    {
        public static void Main()
        {
            using (var context = new AppContext())
            {
                // Инициализация данных
                context.SeedData();

                // Получение студентов с курсами (Eager Loading)
                var studentsWithCourses = context.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                    .ToList();

                Console.WriteLine("Студенты и их курсы:");
                foreach (var student in studentsWithCourses)
                {
                    Console.WriteLine($"Студент: {student.FirstName} {student.LastName}");
                    foreach (var enrollment in student.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }

                // Получение курсов с учителями (Eager Loading)
                var coursesWithTeachers = context.Courses
                    .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                    .Include(c => c.Teacher) // Загружаем учителей вместе с курсами
                    .ToList();

                Console.WriteLine("\nКурсы и их учителя:");
                foreach (var course in coursesWithTeachers)
                {
                    Console.WriteLine($"Курс: {course.CourseName}, Учитель: {course.Teacher.FirstName} {course.Teacher.LastName}");
                    foreach (var enrollment in course.Enrollments)
                    {
                        Console.WriteLine($"  Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                    }
                }

                // Обновление информации о студенте
                var firstStudent = studentsWithCourses.FirstOrDefault();
                if (firstStudent != null)
                {
                    context.UpdateStudent(firstStudent.StudentId, "Максим", "Олифиренко", 20, "Иркутск");
                }

                // Удаление последнего студента
                var lastStudent = studentsWithCourses.LastOrDefault();
                if (lastStudent != null)
                {
                    context.DeleteStudent(lastStudent.StudentId);
                }

                // Повторный вывод списка студентов
                studentsWithCourses = context.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                    .ToList();

                Console.WriteLine("\nОбновленный список студентов:");
                foreach (var student in studentsWithCourses)
                {
                    Console.WriteLine($"Студент: {student.FirstName} {student.LastName}");
                    foreach (var enrollment in student.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }

                // Запрос: Оценки студента по имени "Сергей"
                var sergeyGrades = context.Grades
                    .Where(g => g.Student.FirstName == "Сергей")
                    .Select(g => new
                    {
                        CourseName = g.Course.CourseName,
                        Score = g.Score
                    })
                    .ToList();

                Console.WriteLine("\nОценки Сергея:");
                foreach (var grade in sergeyGrades)
                {
                    Console.WriteLine($"{grade.CourseName}: {grade.Score}");
                }

                // Запрос: Средний балл по курсам
                var averageScores = context.Grades
                    .GroupBy(g => g.Course.CourseName)
                    .Select(g => new
                    {
                        CourseName = g.Key,
                        AverageScore = g.Average(gr => gr.Score)
                    })
                    .ToList();

                Console.WriteLine("\nСредний балл по курсам:");
                foreach (var item in averageScores)
                {
                    Console.WriteLine($"{item.CourseName}: {item.AverageScore:F2}");
                }

                Console.WriteLine();
            }

            // Пример ленивой загрузки
            using (var context = new AppContext())
            {
                context.SeedData();
                Console.WriteLine("--- ЛЕНИВАЯ ЗАГРУЗКА ---");

                var firstStudentLazy = context.Students.FirstOrDefault();
                if (firstStudentLazy != null)
                {
                    Console.WriteLine($"Студент: {firstStudentLazy.FirstName} {firstStudentLazy.LastName}");
                    foreach (var enrollment in firstStudentLazy.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }
            }

            // Пример явной загрузки
            using (var context = new AppContext())
            {
                context.SeedData();
                Console.WriteLine("--- ЯВНАЯ ЗАГРУЗКА ---");

                // Пример явной загрузки
                var firstStudentExplicit = context.Students.FirstOrDefault();
                if (firstStudentExplicit != null)
                {
                    Console.WriteLine($"Студент: {firstStudentExplicit.FirstName} {firstStudentExplicit.LastName}");

                    // Явная загрузка курсов для студента
                    context.Entry(firstStudentExplicit)
                        .Collection(s => s.Enrollments)
                        .Load();

                    foreach (var enrollment in firstStudentExplicit.Enrollments)
                    {
                        Console.WriteLine($"  Курс: {enrollment.Course.CourseName}");
                    }
                }
            }
        }
    }
}

