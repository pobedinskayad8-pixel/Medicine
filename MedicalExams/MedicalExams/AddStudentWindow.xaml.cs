using System;
using System.Windows;
using System.Windows.Controls;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams
{
    public partial class AddStudentWindow : Window
    {

        public AddStudentWindow()
        {
            InitializeComponent();

      
            BirthDatePicker.SelectedDate = DateTime.Now.AddYears(-14);

   
            if (HealthGroupCombo.Items.Count > 0)
                HealthGroupCombo.SelectedIndex = 0;
        }

    
        public AddStudentWindow(Student student) : this()
        {
       
            LastNameBox.Text = student.LastName;
            FirstNameBox.Text = student.FirstName;
            MiddleNameBox.Text = student.MiddleName;
            BirthDatePicker.SelectedDate = student.BirthDate;
            ClassBox.Text = student.ClassName;

      
            foreach (ComboBoxItem item in HealthGroupCombo.Items)
            {
                if (item.Tag.ToString() == student.HealthGroup.ToString())
                {
                    HealthGroupCombo.SelectedItem = item;
                    break;
                }
            }

            PhoneBox.Text = student.Phone;
            ParentPhoneBox.Text = student.ParentPhone;
            AddressBox.Text = student.Address;
            NotesBox.Text = student.Notes;

    
            Title = "Редактирование ученика";
            (FindName("SaveBtn") as Button).Content = "Обновить";
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
      
            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
            {
                MessageBox.Show("Введите фамилию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                LastNameBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
            {
                MessageBox.Show("Введите имя", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                FirstNameBox.Focus();
                return;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                BirthDatePicker.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ClassBox.Text))
            {
                MessageBox.Show("Введите класс", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ClassBox.Focus();
                return;
            }

            if (HealthGroupCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу здоровья", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                HealthGroupCombo.Focus();
                return;
            }

            try
            {
          
                int healthGroup = 1;
                if (HealthGroupCombo.SelectedItem is ComboBoxItem item)
                {
                    healthGroup = int.Parse(item.Tag.ToString());
                }

           
                var student = new Student
                {
                    Id = MedicalData.GetNextStudentId(),
                    LastName = LastNameBox.Text.Trim(),
                    FirstName = FirstNameBox.Text.Trim(),
                    MiddleName = MiddleNameBox.Text?.Trim() ?? "",
                    BirthDate = BirthDatePicker.SelectedDate.Value,
                    ClassName = ClassBox.Text.Trim().ToUpper(),
                    HealthGroup = healthGroup,
                    Phone = PhoneBox.Text?.Trim() ?? "",
                    ParentPhone = ParentPhoneBox.Text?.Trim() ?? "",
                    Address = AddressBox.Text?.Trim() ?? "",
                    Notes = NotesBox.Text?.Trim() ?? ""
                };

          
                MedicalData.Students.Add(student);
                MedicalData.SaveData(); 

     
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
