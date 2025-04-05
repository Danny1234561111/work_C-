using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6.Entities
{
    internal class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public int Duration { get; set; }=0;
        public string? Description { get; set; }
    }
}