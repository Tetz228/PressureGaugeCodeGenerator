namespace PressureGaugeCodeGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using PressureGaugeCodeGenerator.Data;

    using ZXing;
    using ZXing.Common;
    using ZXing.QrCode;

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
                var openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Выберите файл",
                    InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Filter = "Файлы с расширением .txt|*.txt"
                };

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
        /// <param name="path">Путь</param>
        /// <param name="autoSetYear">Установка автоматического года</param>
        /// <param name="department">Участок</param>
        /// <param name="startNumber">Начальный номер</param>
        public static void SetStartNumber(string path, bool? autoSetYear, string department, out string startNumber)
        {
            startNumber = "";
            if (!ChecksFile.EmptyFile(path))
            {
                int lastNumber = int.Parse(File.ReadLines(path).Last());
                if (autoSetYear == false ||
                    int.Parse(GetData.GetYear()) ==
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
                        string newPath = $"{Directory.GetCurrentDirectory()}\\numbers{department}_20{GetData.GetYear()}.txt";
                        File.Create(newPath);
                        startNumber = $"{GetData.GetYear()}{department}000001";
                        streamWriter.Write($"{GetData.GetYear()}{department}000000");
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

        #region Генерация и запись номеров в файл
        /// <summary></summary>
        /// <param name="startNumber"></param>
        /// <param name="countNumber"></param>
        /// <param name="path"></param>
        /// <param name="autoSetYear"></param>
        /// <param name="department"></param>
        public static void Generate(int startNumber, int countNumber, string path, bool? autoSetYear, string department)
        {
            string startNum = startNumber.ToString();
            int num = int.Parse(startNum.Substring(0, 2));

            if (autoSetYear == true && num != int.Parse(GetData.GetYear()))
                MessageBox.Show(
                    $"Вы пытаетесь сгенерировать номера для 20{num}-го года, сейчас 20{GetData.GetYear()}-й год. Чтобы отключить проверку, снимите галочку \"Установить год автоматически\"",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            else
            {
                if (startNum[2] != department[0])
                    startNumber = int.Parse(startNum.Remove(2, 1).Insert(2, department[0].ToString()));

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
                    MessageBox.Show($"Номера успешно сгенерированны и записаны в файл по пути: {path}", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        #endregion

        #region Генерация QR-кодов
        /// <summary>Генерация QR-кодов</summary>
        /// <param name="massNum">Список номеров для генерации</param>
        /// <param name="dataDictionary">Словарь с данными</param>
        public static void GenerateQrCodes(List<string> massNum, Dictionary<string, string> dataDictionary)
        {
            EncodingOptions encodingOptions = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = int.Parse(dataDictionary["Width"]),
                Height = int.Parse(dataDictionary["Height"]),
                Margin = 0
            };
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = encodingOptions
            };
            Bitmap qrCode;

            foreach (var code in massNum)
            {
                qrCode = barcodeWriter.Write(code);
                switch (dataDictionary["Format"])
                {
                    case "BMP":
                        qrCode.Save(GlobalVar.NAME_FOLDER_QR + code + ".bmp", ImageFormat.Bmp);
                        break;
                    case "PNG":
                        qrCode.Save(GlobalVar.NAME_FOLDER_QR + code + ".png", ImageFormat.Png);
                        break;
                    case "JPEG":
                        qrCode.Save(GlobalVar.NAME_FOLDER_QR + code + ".jpeg", ImageFormat.Jpeg);
                        break;
                    case "BMP + PNG":
                        qrCode.Save(GlobalVar.NAME_FOLDER_QR + code + ".png", ImageFormat.Png);

                        encodingOptions.Width = int.Parse(dataDictionary["WidthBmp"]);
                        encodingOptions.Height = int.Parse(dataDictionary["HeightBmp"]);

                        barcodeWriter = new BarcodeWriter
                        {
                            Format = BarcodeFormat.QR_CODE,
                            Options = encodingOptions
                        };

                        Bitmap qrCodeBmp = barcodeWriter.Write(GlobalVar.NAME_FOLDER_QR);
                        qrCodeBmp.Save(dataDictionary["Patch"] + GlobalVar.NAME_FOLDER_QR + ".bmp", ImageFormat.Bmp);
                        break;
                }
            }
        }
        #endregion
    }
}