using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams
{
    public partial class MainWindow : Window
    {
        private List<Student> _allStudents;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _allStudents = MedicalData.Students.ToList();
            UpdateStudentsGrid(_allStudents);
            LoadFilters();
            UpdateStatistics();
            UpdateReminders();
        }

        private void UpdateStudentsGrid(List<Student> students)
        {
            StudentsGrid.ItemsSource = students;
        }

        private void LoadFilters()
        {
       
            var classes = _allStudents.Select(s => s.ClassName).Distinct().OrderBy(c => c).ToList();
            classes.Insert(0, "Все классы");
            ClassFilterCombo.ItemsSource = classes;
            ClassFilterCombo.SelectedIndex = 0;

      
            var healthGroups = new List<string> { "Все", "1", "2", "3", "4", "5" };
            HealthGroupFilterCombo.ItemsSource = healthGroups;
            HealthGroupFilterCombo.SelectedIndex = 0;

  
            var admissionStatuses = new List<string> { "Все", "Действует", "Истекает", "Просрочен", "Нет допуска" };
            AdmissionFilterCombo.ItemsSource = admissionStatuses;
            AdmissionFilterCombo.SelectedIndex = 0;
        }

        private void UpdateStatistics()
        {
            int total = _allStudents.Count;
            StatsText.Text = $"Всего учеников: {total}";
        }

        private void UpdateReminders()
        {
            var expiring = MedicalData.GetStudentsWithExpiringExams();
            var expired = MedicalData.GetStudentsWithExpiredExams();

            if (expired.Any())
            {
                ReminderText.Text = $"⚠ ВНИМАНИЕ! Просроченные справки: {expired.Count} учеников";
            }
            else if (expiring.Any())
            {
                ReminderText.Text = $"⚠ Напоминание: у {expiring.Count} учеников истекают справки";
            }
            else
            {
                ReminderText.Text = "✓ Все справки в порядке";
            }
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
        }

        private void ApplyFilters()
        {
            var filtered = _allStudents.AsEnumerable();


            if (ClassFilterCombo.SelectedItem != null && ClassFilterCombo.SelectedItem.ToString() != "Все классы")
            {
                filtered = filtered.Where(s => s.ClassName == ClassFilterCombo.SelectedItem.ToString());
            }

    
            if (HealthGroupFilterCombo.SelectedItem != null && HealthGroupFilterCombo.SelectedItem.ToString() != "Все")
            {
                int group = int.Parse(HealthGroupFilterCombo.SelectedItem.ToString());
                filtered = filtered.Where(s => s.HealthGroup == group);
            }

    
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string search = SearchBox.Text.ToLower();
                filtered = filtered.Where(s =>
                    s.LastName.ToLower().Contains(search) ||
                    s.FirstName.ToLower().Contains(search) ||
                    s.ClassName.ToLower().Contains(search));
            }

            StudentsGrid.ItemsSource = filtered.ToList();
        }

        private void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddStudentWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                _allStudents = MedicalData.Students.ToList();
                LoadFilters();
                ApplyFilters();
                UpdateStatistics();
                UpdateReminders();
            }
        }

        private void AddExamBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is Student selectedStudent)
            {
                var addWindow = new AddExamWindow(selectedStudent.Id);
                addWindow.Owner = this;
                addWindow.ShowDialog();

                if (addWindow.IsSaved)
                {
              
                    UpdateStatistics();
                    UpdateReminders();
                }
            }
            else
            {
                MessageBox.Show("Выберите ученика из списка", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReportsBtn_Click(object sender, RoutedEventArgs e)
        {
            var reportsWindow = new ReportsWindow();
            reportsWindow.Owner = this;
            reportsWindow.ShowDialog();
        }

        private void ExpiringBtn_Click(object sender, RoutedEventArgs e)
        {
            var expiring = MedicalData.GetStudentsWithExpiringExams();
            string message = "Ученики с истекающими справками (30 дней):\n\n";
            foreach (var student in expiring)
            {
                message += $"{student.FullName} ({student.ClassName})\n";
            }
            MessageBox.Show(message, "Истекающие справки", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExpiredBtn_Click(object sender, RoutedEventArgs e)
        {
            var expired = MedicalData.GetStudentsWithExpiredExams();
            string message = "Ученики с просроченными справками:\n\n";
            foreach (var student in expired)
            {
                message += $"{student.FullName} ({student.ClassName})\n";
            }
            MessageBox.Show(message, "Просроченные справки", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MedicalData.SaveData(); 
        }
    }
}
