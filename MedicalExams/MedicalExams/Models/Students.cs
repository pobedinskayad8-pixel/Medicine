using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalExams.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ClassName { get; set; }
        public int HealthGroup { get; set; } 
        public string Phone { get; set; }
        public string ParentPhone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public virtual List<MedicalExam> Exams { get; set; } = new List<MedicalExam>();

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public string ShortName => $"{LastName} {FirstName[0]}.{MiddleName?[0]}.";
    }
}
