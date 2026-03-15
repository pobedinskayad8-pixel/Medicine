using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MedicalExams.Models;

namespace MedicalExams.Data
{
    public static class DataStorage
    {
        private static readonly string DataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "MedicalExams");

        private static readonly string StudentsFile = Path.Combine(DataFolder, "students.json");
        private static readonly string ExamsFile = Path.Combine(DataFolder, "exams.json");
        private static readonly string CounterFile = Path.Combine(DataFolder, "counters.json");

        static DataStorage()
        {
            // Создаем папку для данных, если её нет
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }
        }

        // Сохранение всех данных
        public static void SaveAll(List<Student> students, List<MedicalExam> exams, int nextStudentId, int nextExamId)
        {
            try
            {
                // Сохраняем учеников
                string studentsJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                File.WriteAllText(StudentsFile, studentsJson);

                // Сохраняем медосмотры
                string examsJson = JsonConvert.SerializeObject(exams, Formatting.Indented);
                File.WriteAllText(ExamsFile, examsJson);

                // Сохраняем счетчики
                var counters = new
                {
                    NextStudentId = nextStudentId,
                    NextExamId = nextExamId
                };
                string countersJson = JsonConvert.SerializeObject(counters, Formatting.Indented);
                File.WriteAllText(CounterFile, countersJson);

                Console.WriteLine("Данные сохранены в " + DataFolder);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения данных: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        // Загрузка учеников
        public static List<Student> LoadStudents()
        {
            try
            {
                if (File.Exists(StudentsFile))
                {
                    string json = File.ReadAllText(StudentsFile);
                    return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки учеников: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            return new List<Student>();
        }

        // Загрузка медосмотров
        public static List<MedicalExam> LoadExams()
        {
            try
            {
                if (File.Exists(ExamsFile))
                {
                    string json = File.ReadAllText(ExamsFile);
                    return JsonConvert.DeserializeObject<List<MedicalExam>>(json) ?? new List<MedicalExam>();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки медосмотров: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            return new List<MedicalExam>();
        }

        // Загрузка счетчиков
        public static (int nextStudentId, int nextExamId) LoadCounters()
        {
            try
            {
                if (File.Exists(CounterFile))
                {
                    string json = File.ReadAllText(CounterFile);
                    var counters = JsonConvert.DeserializeObject<dynamic>(json);
                    return (counters.NextStudentId, counters.NextExamId);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки счетчиков: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            return (1, 1); // Значения по умолчанию
        }

        // Получение пути к папке с данными (для информации)
        public static string GetDataFolderPath()
        {
            return DataFolder;
        }
    }
}
