using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PressureGaugeCodeGeneratorTestWpf.Commands
{
    static class OperationsFiles
    {
        #region Проверка пути файла
        /// <summary>Проверка пути файла</summary>
        /// <param name="path">Строка пути</param>
        /// <returns>Возвращает true, если путь существует, иначе false</returns>
        public static bool CheckPath(string path) => Directory.Exists(Path.GetDirectoryName(path));
        #endregion

        #region Проверка на то, существует ли файл
        /// <summary>Проверка на то, существует ли файл</summary>
        /// <param name="path">Строка пути до файла</param>
        /// <returns>Возвращает true, если путь до файла существует, иначе false</returns>
        public static bool FileExist(string path) => File.Exists(path);
        #endregion

        #region Проверка файла на пустоту
        /// <summary>Проверка файла на пустоту</summary>
        /// <param name="path">Строка пути до файла</param>
        /// <returns>Возвращает true, если файл в себе ничего не содержит, иначе false</returns>
        public static bool EmptyFile(string path) => File.ReadAllLines(path).Length == 0;
        #endregion

        #region Проверка на номер ли в строке
        /// <summary>Проверка на номер ли в строке</summary>
        /// <param name="number">Строка с данными</param>
        /// <returns>Возвращает true, если в строке номер, иначе false</returns>
        public static bool IsNumber(string number) => int.TryParse(number, out _);
        #endregion

        #region Проверка введенного номера в строку и последнего номера в файле
        /// <summary>Проверка введенного номера в строку и последнего номера в файле</summary>
        /// <param name="path">Путь до файла</param>
        /// <param name="number">Номер</param>
        /// <returns>Возвращает true, если введенный номер больше, чем последний номер в файле, иначе false</returns>
        public static bool ValidNumber(string path, string number) => int.Parse(number) > int.Parse(File.ReadLines(path).Last());
        #endregion

        #region Вызов всех проверок файла и пути
        /// <summary>Вызов всех проверок файла и пути</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Возвращает true, если все проверки возвращают true, иначе false</returns>
        public static bool CheckFullPathAndFile(string path) => CheckPath(path) && FileExist(path) && !EmptyFile(path) && IsNumber(File.ReadLines(path).Last());
        #endregion

        #region Получение текущего года с компьютера
        /// <summary>Получение текущего года с компьютера</summary>
        /// <returns>Возвращает последние 2 цифры года</returns>
        public static string GetYear() => DateTime.Now.ToString("yy");
        #endregion

        //public void Generate(int _begin, int _quantity, string _path, int _count)
        //{
        //    string path = _path;
        //    if (_path.Length > 0)
        //    {
        //        if (!CheckPath(_path))
        //        {
        //            MessageBox.Show(
        //                "Путь " + _path + " не найден",
        //                "Ошибка",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            if (checkBox_setYear.IsChecked == false)
        //            {
        //                string year = textBox_begin.Text.Substring(0, 2);
        //                path = Directory.GetCurrentDirectory() + "\\numbers" + GetDepartment() + "_20" + year + ".txt";
        //                textBox_path.Text = path;
        //                //TODO: тут создаём файл с маркировкой года, если проверка года отключена
        //            }
        //            if (!FileExist(path))
        //            {
        //                MessageBoxResult result = MessageBox.Show(
        //                    "Файл " + path + " не существует\nСоздать файл?",
        //                    "Ошибка",
        //                    MessageBoxButton.YesNo,
        //                    MessageBoxImage.Error);

        //                switch (result)
        //                {
        //                    case MessageBoxResult.Yes:
        //                        using (FileStream fs = File.Create(path))
        //                        {
        //                            MessageBox.Show("Создан файл" + path, "Информация");
        //                        }
        //                        Generate(_begin, _quantity, path, _count);
        //                        //     textBox_begin.Text = GetYear() + GetDepartment() + "000001";
        //                        break;
        //                    case MessageBoxResult.No:
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                int num = Int32.Parse(textBox_begin.Text.Substring(0, 2));
        //                if (checkBox_setYear.IsChecked == true && num != Int32.Parse(GetYear()))
        //                {
        //                    MessageBox.Show(
        //                        "Вы пытаетесь сгенерировать номера для 20" + num + "-го года, " +
        //                        "сейчас 20" + Int32.Parse(GetYear()) + "-й год. " +
        //                        "Чтобы отключить проверку, снимите галочку \"Установить год автоматически\"",
        //                        "Ошибка",
        //                        MessageBoxButton.OK,
        //                        MessageBoxImage.Error);
        //                }
        //                else
        //                {
        //                    string str = _begin.ToString();
        //                    string department = GetDepartment();
        //                    if (str[2] != department[0])
        //                    {
        //                        str = str.Remove(2, 1).Insert(2, department[0].ToString());
        //                        _begin = Int32.Parse(str);
        //                    }
        //                    WriteFile(path, _begin, _quantity, _count);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show(
        //                "Путь к файлу не должен быть пустым",
        //                "Ошибка",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Error);
        //    }
        //}
    }
}
