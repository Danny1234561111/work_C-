using WebApplication1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
    }
}