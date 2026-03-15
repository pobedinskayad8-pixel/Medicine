using System;

namespace MedicalExams.Models
{
    public class MedicalExam
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

     
        public bool IsExpiringSoon
        {
            get
            {
                return ExpiryDate <= DateTime.Now.AddDays(30) && ExpiryDate > DateTime.Now;
            }
        }

        public bool IsExpired
        {
            get
            {
                return ExpiryDate < DateTime.Now;
            }
        }

    
        public virtual Student Student { get; set; }
    }
}
