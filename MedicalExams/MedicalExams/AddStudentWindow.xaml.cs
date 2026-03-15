using System;
using System.Windows;
using System.Windows.Controls;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams
{
    public partial class AddStudentWindow : Window
    {
        // Конструктор для добавления нового ученика
        public AddStudentWindow()
        {
            InitializeComponent();

            // Устанавливаем дату рождения по умолчанию (примерно 14 лет назад)
            BirthDatePicker.SelectedDate = DateTime.Now.AddYears(-14);

            // Выбираем первую группу здоровья по умолчанию
            if (HealthGroupCombo.Items.Count > 0)
                HealthGroupCombo.SelectedIndex = 0;
        }

        // Конструктор для редактирования существующего ученика
        public AddStudentWindow(Student student) : this()
        {
            // Заполняем поля данными ученика
            LastNameBox.Text = student.LastName;
            FirstNameBox.Text = student.FirstName;
            MiddleNameBox.Text = student.MiddleName;
            BirthDatePicker.SelectedDate = student.BirthDate;
            ClassBox.Text = student.ClassName;

            // Выбираем соответствующую группу здоровья
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

            // Меняем заголовок окна
            Title = "Редактирование ученика";
            (FindName("SaveBtn") as Button).Content = "Обновить";
        }

        // Обработчик кнопки Сохранить
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Валидация обязательных полей
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
                // Получаем выбранную группу здоровья
                int healthGroup = 1;
                if (HealthGroupCombo.SelectedItem is ComboBoxItem item)
                {
                    healthGroup = int.Parse(item.Tag.ToString());
                }

                // Создаем нового ученика
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

                // Добавляем в хранилище
                MedicalData.Students.Add(student);
                MedicalData.SaveData(); // Сохраняем изменения

                // Успешное завершение
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки Отмена
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
