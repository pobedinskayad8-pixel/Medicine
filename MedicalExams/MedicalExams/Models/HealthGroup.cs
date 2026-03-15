using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalExams.Models
{
    public static class HealthGroup
    {
        public static string GetDescription(int group)
        {
            switch (group)
            {
                case 1:
                    return "I группа - здоровы";
                case 2:
                    return "II группа - с незначительными отклонениями";
                case 3:
                    return "III группа - хронические заболевания в компенсации";
                case 4:
                    return "IV группа - хронические заболевания в субкомпенсации";
                case 5:
                    return "V группа - инвалидность";
                default:
                    return "Не определена";
            }
        }
    }
}