using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalExams.Models
{
    public class MedicalExams
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExamType { get; set; } 
        public string DoctorName { get; set; }
        public string Conclusion { get; set; }
        public string Restrictions { get; set; }
        public bool IsAdmitted { get; set; }
        public string DocumentNumber { get; set; }
        public string Notes { get; set; }

        public bool IsExpiringSoon => ExpiryDate <= DateTime.Now.AddDays(30) && ExpiryDate > DateTime.Now;
        public bool IsExpired => ExpiryDate < DateTime.Now;

        public virtual Student Student { get; set; }
    }
}
