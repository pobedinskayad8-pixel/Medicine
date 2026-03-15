using System;
using System.Collections.Generic;
using System.Linq;
using MedicalExams.Models;

namespace MedicalExams.Data
{
    public static class MedicalData
    {
        public static List<Student> Students { get; set; } = new List<Student>();
        public static List<MedicalExam> Exams { get; set; } = new List<MedicalExam>();

        private static int _nextStudentId = 1;
        private static int _nextExamId = 1;

        static MedicalData()
        {
            LoadData();
        }

        // Загрузка данных из файлов
        private static void LoadData()
        {
            try
            {
                // Загружаем учеников
                Students = DataStorage.LoadStudents();

                // Загружаем медосмотры
                Exams = DataStorage.LoadExams();

                // Загружаем счетчики
                var (nextStudentId, nextExamId) = DataStorage.LoadCounters();
                _nextStudentId = nextStudentId;
                _nextExamId = nextExamId;

                // Если нет данных, добавляем тестовые
                if (!Students.Any())
                {
                    AddTestData();
                    SaveData(); // Сразу сохраняем тестовые данные
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки данных: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                // Если ошибка, создаем тестовые данные
                Students.Clear();
                Exams.Clear();
                _nextStudentId = 1;
                _nextExamId = 1;
                AddTestData();
            }
        }

        // Сохранение данных в файлы
        public static void SaveData()
        {
            DataStorage.SaveAll(Students, Exams, _nextStudentId, _nextExamId);
        }

        private static void AddTestData()
        {
            // Тестовые ученики
            var student1 = new Student
            {
                Id = _nextStudentId++,
                LastName = "Иванов",
                FirstName = "Иван",
                MiddleName = "Иванович",
                BirthDate = new DateTime(2010, 5, 15),
                ClassName = "9А",
                HealthGroup = 2,
                Phone = "8-999-123-45-67",
                ParentPhone = "8-999-765-43-21",
                Address = "ул. Ленина, д. 10, кв. 25"
            };

            var student2 = new Student
            {
                Id = _nextStudentId++,
                LastName = "Петрова",
                FirstName = "Анна",
                MiddleName = "Сергеевна",
                BirthDate = new DateTime(2011, 3, 20),
                ClassName = "8Б",
                HealthGroup = 1,
                Phone = "8-999-234-56-78",
                ParentPhone = "8-999-876-54-32",
                Address = "ул. Гагарина, д. 5, кв. 12"
            };

            var student3 = new Student
            {
                Id = _nextStudentId++,
                LastName = "Сидоров",
                FirstName = "Петр",
                MiddleName = "Алексеевич",
                BirthDate = new DateTime(2010, 11, 10),
                ClassName = "9А",
                HealthGroup = 3,
                Phone = "8-999-345-67-89",
                ParentPhone = "8-999-987-65-43",
                Address = "ул. Пушкина, д. 15, кв. 7"
            };

            Students.Add(student1);
            Students.Add(student2);
            Students.Add(student3);

            // Тестовые медосмотры
            Exams.Add(new MedicalExam
            {
                Id = _nextExamId++,
                StudentId = student1.Id,
                ExamDate = new DateTime(2025, 9, 1),
                ExpiryDate = new DateTime(2026, 9, 1),
                ExamType = "Ежегодный",
                DoctorName = "Петров И.И.",
                Conclusion = "Здоров",
                IsAdmitted = true,
                DocumentNumber = "СПР-2025-001"
            });

            Exams.Add(new MedicalExam
            {
                Id = _nextExamId++,
                StudentId = student1.Id,
                ExamDate = new DateTime(2026, 2, 15),
                ExpiryDate = new DateTime(2026, 5, 15),
                ExamType = "Перед соревнованиями",
                DoctorName = "Сидорова А.А.",
                Conclusion = "Допущен",
                Restrictions = "Без ограничений",
                IsAdmitted = true,
                DocumentNumber = "СПР-2026-042"
            });

            Exams.Add(new MedicalExam
            {
                Id = _nextExamId++,
                StudentId = student2.Id,
                ExamDate = new DateTime(2025, 8, 25),
                ExpiryDate = new DateTime(2026, 8, 25),
                ExamType = "Ежегодный",
                DoctorName = "Петров И.И.",
                Conclusion = "Здорова",
                IsAdmitted = true,
                DocumentNumber = "СПР-2025-089"
            });

            Exams.Add(new MedicalExam
            {
                Id = _nextExamId++,
                StudentId = student3.Id,
                ExamDate = new DateTime(2025, 5, 10),
                ExpiryDate = new DateTime(2026, 5, 10),
                ExamType = "Ежегодный",
                DoctorName = "Петров И.И.",
                Conclusion = "Хронический гастрит",
                Restrictions = "Щадящее питание в столовой",
                IsAdmitted = true,
                DocumentNumber = "СПР-2025-156"
            });
        }

        public static int GetNextStudentId()
        {
            return _nextStudentId++;
        }

        public static int GetNextExamId()
        {
            return _nextExamId++;
        }

        // Получение всех осмотров ученика
        public static List<MedicalExam> GetStudentExams(int studentId)
        {
            return Exams.Where(e => e.StudentId == studentId).OrderByDescending(e => e.ExamDate).ToList();
        }

        // Получение действующего допуска
        public static MedicalExam GetCurrentAdmission(int studentId)
        {
            return Exams.Where(e => e.StudentId == studentId && e.IsAdmitted && e.ExpiryDate >= DateTime.Now)
                       .OrderByDescending(e => e.ExamDate)
                       .FirstOrDefault();
        }

        // Ученики с истекающими справками
        public static List<Student> GetStudentsWithExpiringExams(int days = 30)
        {
            var expiringExamStudentIds = Exams.Where(e => e.ExpiryDate <= DateTime.Now.AddDays(days) &&
                                                          e.ExpiryDate > DateTime.Now)
                                              .Select(e => e.StudentId)
                                              .Distinct()
                                              .ToList();

            return Students.Where(s => expiringExamStudentIds.Contains(s.Id)).ToList();
        }

        // Ученики с просроченными справками
        public static List<Student> GetStudentsWithExpiredExams()
        {
            var expiredExamStudentIds = Exams.Where(e => e.ExpiryDate < DateTime.Now)
                                             .Select(e => e.StudentId)
                                             .Distinct()
                                             .ToList();

            return Students.Where(s => expiredExamStudentIds.Contains(s.Id)).ToList();
        }
    }
}
