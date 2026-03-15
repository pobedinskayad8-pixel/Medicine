using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MedicalExams.Data;
using MedicalExams.Models;

namespace MedicalExams.Converters
{
    // Конвертер для получения статуса допуска
    public class AdmissionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Student student)
            {
                var currentExam = MedicalData.GetCurrentAdmission(student.Id);
                if (currentExam == null)
                    return "Нет допуска";

                if (currentExam.IsExpired)
                    return "Просрочен";

                if (currentExam.IsExpiringSoon)
                    return "Истекает";

                return "Действует";
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Конвертер для проверки наличия действующего допуска
    public class HasCurrentAdmissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Student student)
            {
                var exam = MedicalData.GetCurrentAdmission(student.Id);
                return exam != null && !exam.IsExpired;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Конвертер для проверки истекающего допуска
    public class HasExpiringAdmissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Student student)
            {
                var exam = MedicalData.GetCurrentAdmission(student.Id);
                return exam != null && exam.IsExpiringSoon;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Конвертер для проверки просроченного допуска
    public class HasExpiredAdmissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Student student)
            {
                var exam = MedicalData.GetCurrentAdmission(student.Id);
                return exam != null && exam.IsExpired;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Конвертер для получения списка осмотров ученика
    public class StudentExamsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Student student)
            {
                return MedicalData.GetStudentExams(student.Id);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}