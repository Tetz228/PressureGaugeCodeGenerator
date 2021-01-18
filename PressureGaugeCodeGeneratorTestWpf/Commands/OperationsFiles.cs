using PressureGaugeCodeGeneratorTestWpf.Data;
using PressureGaugeCodeGeneratorTestWpf.Windows;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using form = System.Windows.Forms;

namespace PressureGaugeCodeGeneratorTestWpf.Commands
{
    internal static class OperationsFiles
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

        //Мб переписать метод
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

        #region Открытие .txt файла
        /// <summary>Открытие .txt файла</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Возвращает true, если файл выбран, иначе false</returns>
        public static bool OpenFile(out string path)
        {
            path = "";
            form.OpenFileDialog openFileDialog;
            try
            {
                openFileDialog = new form.OpenFileDialog
                {
                    Title = "Выберите файл",
                    InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Filter = "Файлы с расширением .txt|*.txt"
                };
                if (openFileDialog.ShowDialog() == form.DialogResult.OK)
                {
                    if (!EmptyFile(openFileDialog.FileName))
                        if (!IsNumber(File.ReadLines(openFileDialog.FileName).Last()))
                        {
                            MessageBox.Show($"Неверный формат файла - {openFileDialog.FileName}\nФайл должен содержать {GlobalVar.DIGITS}-значные номера",
                                            "Некорректный формат файла", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                    path = openFileDialog.FileName;
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при выборе файла!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
        #endregion

        #region Установка начального значения
        /// <summary>Установка начального значения</summary>
        /// <param name="data">Кортеж с данными о пути, о установки автоматического года, о участке</param>
        /// <param name="startNumber">Начальный номер</param>
        public static void SetStartNumber((string path, bool? autoSetYear, string department) data, out string startNumber)
        {
            startNumber = "";
            if (!EmptyFile(data.path))
            {
                int lastNumber = int.Parse(File.ReadLines(data.path).Last());
                if (int.Parse(GetYear()) == int.Parse(lastNumber.ToString().Substring(0, 2)))
                    startNumber = lastNumber++.ToString();
                else
                {
                    if (data.autoSetYear == true)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(data.path))
                        {
                            string newPath = $"{Directory.GetCurrentDirectory()}\\numbers{data.department}_20{GetYear()}.txt";
                            File.Create(newPath);
                            startNumber = $"{GetYear()}{data.department}000001";
                            streamWriter.Write($"{GetYear()}{data.department}000000");
                        }
                        MessageBox.Show($"Настал следующий год.\nПервые цифры номера теперь - {int.Parse(lastNumber.ToString().Substring(0, 2))}", "Информация",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                }
            }
        }
        #endregion

        #region Чтение файла с номерами
        /// <summary>Чтение файла с номерами</summary>
        /// <param name="path">Путь до файла</param>
        public static void ReadingFileNumbers(string path)
        {
            if (!CheckPath(path))
                return;
            if (!FileExist(path))
                return;

            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    ListNumbersWindow numbersWindow = new ListNumbersWindow();
                    string numbers = streamReader.ReadToEnd();
                    numbersWindow.TextBoxNumbers.Text = numbers == "" ? "Номера в файле отсутствуют!" : numbers;
                    numbersWindow.Show();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при чтении файла с номерами", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Вывод номеров в интерфейсе, которые будут сгенерированные для QR-кодов
        /// <summary>Вывод номеров в интерфейсе, которые будут сгенерированные для QR-кодов</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Вывод первого и последнего номера,или одно номера, если первый и последний номер совпадают, или вывод "Нет номеров для генерации!"</returns>
        public static string DrawNumbers(string path)
        {
            if (!EmptyFile(path))
            {
                string first = int.Parse(File.ReadLines(path).First()).ToString();
                string last = int.Parse(File.ReadLines(path).Last()).ToString();

                if (first.Equals(last))
                    return first;

                return first + " - " + last;
            }

            return "Нет номеров для генерации!";
        }
        #endregion

        //public void Generate(int _begin, int _quantity, string path, int _count)
        //{
        //    string directory = path;
        //    if (path.Length > 0)
        //    {
        //        if (!CheckPath(path))
        //        {
        //            MessageBox.Show(
        //                "Путь " + path + " не найден",
        //                "Ошибка",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            if (checkBox_setYear.IsChecked == false)
        //            {
        //                string year = textBox_begin.Text.Substring(0, 2);
        //                directory = Directory.GetCurrentDirectory() + "\\numbers" + GetDepartment() + "_20" + year + ".txt";
        //                textBox_path.Text = directory;
        //                //TODO: тут создаём файл с маркировкой года, если проверка года отключена
        //            }
        //            if (!FileExist(directory))
        //            {
        //                MessageBoxResult result = MessageBox.Show(
        //                    "Файл " + directory + " не существует\nСоздать файл?",
        //                    "Ошибка",
        //                    MessageBoxButton.YesNo,
        //                    MessageBoxImage.Error);

        //                switch (result)
        //                {
        //                    case MessageBoxResult.Yes:
        //                        using (FileStream fs = File.Create(directory))
        //                        {
        //                            MessageBox.Show("Создан файл" + directory, "Информация");
        //                        }
        //                        Generate(_begin, _quantity, directory, _count);
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
        //                    WriteFile(directory, _begin, _quantity, _count);
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

        #region Запись номеров в файл
        /// <summary>Запись номеров в файл</summary>
        /// <param name="path">Путь до файла</param>
        /// <param name="startNumber">Начальный номер</param>
        /// <param name="countNumber">Количество номеров</param>
        public static void WritingNumbersToFile(string path, int startNumber, int countNumber)
        {
            int[] arr = new int[countNumber];
            arr[0] = startNumber;

            for (int i = 1; i < countNumber; i++) 
                arr[i] = ++startNumber;

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                for (int i = 0; i < countNumber; i++)
                {
                    if (i < countNumber - 1)
                        streamWriter.WriteLine(arr[i].ToString());
                    else
                        streamWriter.Write(arr[i].ToString());
                }
                MessageBox.Show($"Номера успешно сгенерированны и записаны в файл: {path}", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //SetStartNumber(_path, _count);
        }
        #endregion

        public static string Handle(string department)
        {
            string path = "";

            switch (department)
            {
                case "1 - Литография (ППШ)":
                    ComboBoxOpenFile(path = $"{Directory.GetCurrentDirectory()}\\numbers1_20{GetYear()}.txt");
                    break;
                case "2 - Безрегулировка":
                    ComboBoxOpenFile(path = $"{Directory.GetCurrentDirectory()}\\numbers2_20{GetYear()}.txt");
                    break;
                case "3 - Безрегулировка (штучный циферблат)":
                    ComboBoxOpenFile(path = $"{Directory.GetCurrentDirectory()}\\numbers3_20{GetYear()}.txt");
                    break;
                case "4 - ПНП":
                    ComboBoxOpenFile(path = $"{Directory.GetCurrentDirectory()}\\numbers4_20{GetYear()}.txt");
                    break;
            }

            return path;
        }

        public static void ComboBoxOpenFile(string path)
        {
            //if (CheckFullPathAndFile(path))
            //{
            //    textBox_path.Text = path;
            //    SetBeginNumber(textBox_path.Text, GlobalVar.DIGITS);
            //}
            //else if (CheckPath(path) && FileExist(path) && EmptyFile(path))
            //{
            //    textBox_begin.Text = GetYear() + GetDepartment() + "000001";
            //}
            //else if (!FileExist(path))
            //{
            //    MessageBoxResult result = MessageBox.Show(
            //        "Файл " + path + " не существует\nСоздать файл?",
            //        "Ошибка",
            //        MessageBoxButton.YesNo,
            //        MessageBoxImage.Error);

            //    switch (result)
            //    {
            //        case MessageBoxResult.Yes:
            //            using (FileStream fs = File.Create(path))
            //            {
            //                textBox_begin.Text = GetYear() + GetDepartment() + "000001";
            //                MessageBox.Show("Создан файл" + path, "Информация");
            //            }
            //            break;
            //        case MessageBoxResult.No:
            //            break;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(
            //        "Файл " + path + " содержит некорректные данные\n" +
            //        "Файл должен содержать " + GlobalVar.DIGITS + "-значные номера",
            //        "Ошибка",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    textBox_begin.Text = GetYear() + GetDepartment() + "000001";
            //}
        }
    }
}


