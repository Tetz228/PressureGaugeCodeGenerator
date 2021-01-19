using System;
using System.Windows.Controls;

namespace PressureGaugeCodeGeneratorTestWpf.Classes
{
    static class GetData
    {
        #region Получение текущего года с компьютера
        /// <summary>Получение текущего года с компьютера</summary>
        /// <returns>Возвращает последние 2 цифры года</returns>
        public static string GetYear() => DateTime.Now.ToString("yy");
        #endregion

        #region Получение номера, выбранного участка
        /// <summary>Получение номера, выбранного участка</summary>
        /// <returns>Номер участка</returns>
        public static string GetDepartment(ComboBox department) => department.SelectedItem.ToString().Remove(0, 38).Remove(1);
        #endregion
    }
}
