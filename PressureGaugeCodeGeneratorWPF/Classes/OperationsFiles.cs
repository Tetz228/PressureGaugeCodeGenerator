﻿namespace PressureGaugeCodeGenerator.Classes
{
    using PressureGaugeCodeGenerator.Data;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;

    using MessageBox = System.Windows.MessageBox;

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
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Выберите файл",
                    InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Filter = "Файлы с расширением .txt|*.txt"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Checks.EmptyFile(openFileDialog.FileName))
                        if (!Checks.IsNumber(File.ReadLines(openFileDialog.FileName).Last()))
                        {
                            MessageBox.Show($"Неверный формат файла - {openFileDialog.FileName}\nФайл должен содержать {Data.DIGITS_IN_NUMBER}-значные номера",
                                            "Некорректный формат файла",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                            return false;
                        }

                    path = openFileDialog.FileName;
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                                "Ошибка при выборе файла!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            return false;
        }
        #endregion

        #region Установка начального значения
        /// <summary>Установка начального значения</summary>
        /// <param name="path">Путь</param>
        /// <param name="autoSetYear">Установка автоматического года</param>
        /// <param name="department">Участок</param>
        /// <param name="startNumber">Начальный номер</param>
        public static void SetStartNumber(string path, bool? autoSetYear, string department, out string startNumber)
        {
            startNumber = "";
            if (Checks.EmptyFile(path))
            {
                int lastNumber = int.Parse(File.ReadLines(path).Last());
                if (autoSetYear == false ||
                    int.Parse(Data.GetYear()) ==
                    int.Parse(lastNumber.ToString().Substring(0, 2)))
                {
                    lastNumber++;
                    startNumber = lastNumber.ToString();
                    return;
                }

                if (autoSetYear == true)
                {
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        string newPath = $"{Directory.GetCurrentDirectory()}\\numbers{department}_20{Data.GetYear()}.txt";
                        File.Create(newPath).Dispose();
                        startNumber = $"{Data.GetYear()}{department}000001";
                        streamWriter.Write($"{Data.GetYear()}{department}000000");
                    }
                    MessageBox.Show($"Настал следующий год.\nПервые цифры номера теперь - {int.Parse(lastNumber.ToString().Substring(0, 2))}", "Информация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
            }
        }
        #endregion

        #region Вывод номеров в интерфейсе, которые будут сгенерированные для QR-кодов
        /// <summary>Вывод номеров в интерфейсе, которые будут сгенерированные для QR-кодов</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Вывод первого и последнего номера,или одно номера, если первый и последний номер совпадают, или вывод "Нет номеров для генерации!"</returns>
        public static string DrawNumbers(string path)
        {
            if (Checks.EmptyFile(path))
            {
                string firstNumber = int.Parse(File.ReadLines(path).First()).ToString();
                string lastNumber = int.Parse(File.ReadLines(path).Last()).ToString();

                if (firstNumber.Equals(lastNumber))
                    return firstNumber;

                return firstNumber + " - " + lastNumber;
            }

            return "Нет номеров для генерации!";
        }
        #endregion

        #region Генерация и запись номеров в файл
        /// <summary>Генерация и запись номеров в файл</summary>
        /// <param name="startNumber">Начальный номер</param>
        /// <param name="countNumber">Количество начальных номеров</param>
        /// <param name="path">Путь до файла с номерами</param>
        /// <param name="autoSetYear">Установка автоматического года</param>
        /// <param name="department">Отдел</param>
        public static void Generate(int startNumber, int countNumber, string path, bool? autoSetYear, string department)
        {
            string startNum = startNumber.ToString();
            int year = int.Parse(startNum.Substring(0, 2));

            if (autoSetYear == true && year != int.Parse(Data.GetYear()))
                MessageBox.Show(
                    $"Вы пытаетесь сгенерировать номера для 20{year}-го года, сейчас 20{Data.GetYear()}-й год. Чтобы отключить проверку, снимите галочку \"Установить год автоматически\"",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            else
            {
                if (startNum[2] != department[0])
                    startNumber = int.Parse(startNum.Remove(2, 1).Insert(2, department[0].ToString()));

                int[] massNumbers = new int[countNumber];
                massNumbers[0] = startNumber;

                for (int i = 1; i < countNumber; i++)
                    massNumbers[i] = ++startNumber;

                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    for (int i = 0; i < countNumber; i++)
                    {
                        if (i < countNumber - 1)
                            streamWriter.WriteLine(massNumbers[i].ToString());
                        else
                            streamWriter.Write(massNumbers[i].ToString());
                    }
                    MessageBox.Show($"Номера успешно сгенерированны и записаны в файл по пути: {path}", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        #endregion

        public static void FileCreate()
        {
            if (!Checks.FileExist(Data.PathBaseNumbers))
                File.Create(Data.PathBaseNumbers);
        }

        public static void DirectoryCreate()
        {
            if (!Checks.DirectoryExist())
                Directory.CreateDirectory(Data.PathQrCode);
        }

        public static void CleaningDirectory()
        {
            string[] filesInFolder = Directory.GetFiles(Data.PathQrCode, "*.*");
            foreach (string file in filesInFolder)
                File.Delete(file);
        }
    }
}