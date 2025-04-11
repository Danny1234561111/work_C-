using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        [Required]
        public string? CourseName { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Grade>? Grades { get; set; } = new List<Grade>();

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}