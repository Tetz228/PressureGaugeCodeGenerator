using PressureGaugeCodeGeneratorTestWpf.Data;
using PressureGaugeCodeGeneratorTestWpf.Windows;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using form = System.Windows.Forms;

namespace PressureGaugeCodeGeneratorTestWpf.Classes
{
    internal static class OperationsFiles
    {
        #region Открытие .txt файла
        /// <summary>Открытие .txt файла</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Возвращает true, если файл выбран, иначе false</returns>
        public static bool OpenFile(out string path)
        {
            path = "";
            try
            {
                var openFileDialog = new form.OpenFileDialog
                {
                    Title = "Выберите файл",
                    InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Filter = "Файлы с расширением .txt|*.txt"
                };

                if (openFileDialog.ShowDialog() == form.DialogResult.OK)
                {
                    if (!ChecksFile.EmptyFile(openFileDialog.FileName))
                        if (!ChecksFile.IsNumber(File.ReadLines(openFileDialog.FileName).Last()))
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
            if (!ChecksFile.EmptyFile(data.path))
            {
                int lastNumber = int.Parse(File.ReadLines(data.path).Last());
                if (data.autoSetYear == false ||
                    int.Parse(GetData.GetYear()) ==
                    int.Parse(lastNumber.ToString().Substring(0, 2)))
                {
                    startNumber = lastNumber++.ToString();
                    return;
                }

                if (data.autoSetYear == true)
                {
                    using (StreamWriter streamWriter = new StreamWriter(data.path))
                    {
                        string newPath = $"{Directory.GetCurrentDirectory()}\\numbers{data.department}_20{GetData.GetYear()}.txt";
                        File.Create(newPath);
                        startNumber = $"{GetData.GetYear()}{data.department}000001";
                        streamWriter.Write($"{GetData.GetYear()}{data.department}000000");
                    }
                    MessageBox.Show($"Настал следующий год.\nПервые цифры номера теперь - {int.Parse(lastNumber.ToString().Substring(0, 2))}", "Информация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
            }
        }
        #endregion

        #region Чтение файла с номерами
        /// <summary>Чтение файла с номерами</summary>
        /// <param name="path">Путь до файла</param>
        public static void ReadingFileNumbers(string path)
        {
            if (!ChecksFile.CheckPath(path))
                return;
            if (!ChecksFile.FileExist(path))
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
            if (!ChecksFile.EmptyFile(path))
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

        public static void Generate(int startNumber, int countNumber, string path, bool? autoSetYear)
        {
            //if (autoSetYear == false)
            //{
            //    string year = startNumber.ToString().Substring(0, 2);
            //    path = Directory.GetCurrentDirectory() + "\\numbers" + GetDepartment() + "_20" + year + ".txt";
            //    textBox_path.Text = path;
            //    //TODO: тут создаём файл с маркировкой года, если проверка года отключена
            //}
            //else
            //{
            //    int num = Int32.Parse(textBox_begin.Text.Substring(0, 2));
            //    if (checkBox_setYear.IsChecked == true && num != Int32.Parse(GetYear()))
            //    {
            //        MessageBox.Show(
            //            "Вы пытаетесь сгенерировать номера для 20" + num + "-го года, " +
            //            "сейчас 20" + Int32.Parse(GetYear()) + "-й год. " +
            //            "Чтобы отключить проверку, снимите галочку \"Установить год автоматически\"",
            //            "Ошибка",
            //            MessageBoxButton.OK,
            //            MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        string str = startNumber.ToString();
            //        string department = GetDepartment();
            //        if (str[2] != department[0])
            //        {
            //            str = str.Remove(2, 1).Insert(2, department[0].ToString());
            //            startNumber = Int32.Parse(str);
            //        }
            //        WriteFile(path, startNumber, countNumber, GlobalVarDIGITS);
            //    }
            //}
        }

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
    }
}