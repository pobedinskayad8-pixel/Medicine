using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams
{
    public partial class AddExamWindow : Window
    {
        private int _studentId;
        private Student _student;
        private bool _isSaved = false;

        public AddExamWindow(int studentId)
        {
            InitializeComponent();
            _studentId = studentId;
            _student = MedicalData.Students.FirstOrDefault(s => s.Id == studentId);

            if (_student != null)
            {
                StudentInfoTextBlock.Text = $"{_student.FullName} ({_student.ClassName})";
                TitleTextBlock.Text = $"МЕДОСМОТР: {_student.FullName}";
            }


            ExamDatePicker.SelectedDate = DateTime.Today;
            ExpiryDatePicker.SelectedDate = DateTime.Today.AddYears(1);

     
            if (ExamTypeComboBox.Items.Count > 0)
                ExamTypeComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
   
            if (ExamDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату осмотра", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ExpiryDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату окончания действия", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ExamTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип осмотра", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ConclusionTextBox.Text))
            {
                MessageBox.Show("Введите заключение", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
         
                string examType = "Ежегодный";
                if (ExamTypeComboBox.SelectedItem is ComboBoxItem item)
                {
                    examType = item.Content.ToString();
                }

                var exam = new MedicalExam
                {
                    Id = MedicalData.GetNextExamId(),
                    StudentId = _studentId,
                    ExamDate = ExamDatePicker.SelectedDate.Value,
                    ExpiryDate = ExpiryDatePicker.SelectedDate.Value,
                    ExamType = examType,
                    DoctorName = DoctorTextBox.Text?.Trim() ?? "",
                    Conclusion = ConclusionTextBox.Text.Trim(),
                    Restrictions = RestrictionsTextBox.Text?.Trim() ?? "",
                    IsAdmitted = AdmittedCheckBox.IsChecked ?? true,
                    DocumentNumber = DocumentTextBox.Text?.Trim() ?? "",
                    Notes = NotesTextBox.Text?.Trim() ?? ""
                };

                MedicalData.Exams.Add(exam);
                MedicalData.SaveData(); 

                MessageBox.Show("Медосмотр успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _isSaved = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _isSaved = false;
            this.Close();
        }

 
        public bool IsSaved
        {
            get { return _isSaved; }
        }
    }
}
