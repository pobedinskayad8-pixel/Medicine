using System;
using System.Linq;
using System.Windows;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            try
            {
                // Общая статистика
                int totalStudents = MedicalData.Students.Count;
                int totalExams = MedicalData.Exams.Count;
                int activeExams = MedicalData.Students.Count(s => MedicalData.GetCurrentAdmission(s.Id) != null);
                int expiringCount = MedicalData.GetStudentsWithExpiringExams().Count;
                int expiredCount = MedicalData.GetStudentsWithExpiredExams().Count;

                TotalStudentsText.Text = $"Всего учеников: {totalStudents}";
                TotalExamsText.Text = $"Всего медосмотров: {totalExams}";
                ActiveExamsText.Text = $"Действующих допусков: {activeExams}";
                ExpiringExamsText.Text = $"Истекают в ближайшие 30 дней: {expiringCount}";
                ExpiredExamsText.Text = $"Просрочено: {expiredCount}";

                // Группы здоровья
                HealthGroup1Text.Text = $"Группа 1: {MedicalData.Students.Count(s => s.HealthGroup == 1)}";
                HealthGroup2Text.Text = $"Группа 2: {MedicalData.Students.Count(s => s.HealthGroup == 2)}";
                HealthGroup3Text.Text = $"Группа 3: {MedicalData.Students.Count(s => s.HealthGroup == 3)}";
                HealthGroup4Text.Text = $"Группа 4: {MedicalData.Students.Count(s => s.HealthGroup == 4)}";
                HealthGroup5Text.Text = $"Группа 5: {MedicalData.Students.Count(s => s.HealthGroup == 5)}";

                // Классы
                var classes = MedicalData.Students
                    .Select(s => s.ClassName)
                    .Distinct()
                    .OrderBy(c => c)
                    .Select(c => $"{c}: {MedicalData.Students.Count(s => s.ClassName == c)} учеников")
                    .ToList();

                ClassesListBox.ItemsSource = classes;

                // Таблица учеников
                StudentsReportGrid.ItemsSource = MedicalData.Students.ToList();

                // Просроченные справки
                var expiredStudents = MedicalData.GetStudentsWithExpiredExams();
                ExpiredReportGrid.ItemsSource = expiredStudents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки отчетов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportStudentsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция экспорта в Excel будет доступна в следующей версии",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PrintStudentsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция печати будет доступна в следующей версии",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NotifyExpiredBtn_Click(object sender, RoutedEventArgs e)
        {
            var expired = MedicalData.GetStudentsWithExpiredExams();
            if (expired.Any())
            {
                MessageBox.Show($"Отправлено напоминаний: {expired.Count} (демо-режим)",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Нет учеников с просроченными справками",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshReportBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadReports();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
