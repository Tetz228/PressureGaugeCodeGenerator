namespace PressureGaugeCodeGenerator.Classes
{
    using PressureGaugeCodeGenerator.Data;
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Xml;
    using ZXing;
    using ZXing.Common;
    using ZXing.QrCode;

    static class OperationsQrCodes
    {
        #region Расшифровка QR-кода
        /// <summary>Расшифровка QR-кода</summary>
        /// <param name="pscanData"></param>
        /// <returns>Расшифрованный номер</returns>
        public static void DecodingQrCode(ref string pscanData, out string decodedNumber)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(pscanData);

            string decoder = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
            string[] numbers = decoder.Split(' ');
            string strData = decodedNumber = "";

            foreach (var number in numbers)
            {
                if (string.IsNullOrEmpty(number))
                    break;

                strData += ((char)Convert.ToInt32(number, 16)).ToString();
            }

            decodedNumber = strData;
        }
        #endregion

        #region Генерация QR-кодов
        /// <summary>Генерация QR-кодов</summary>
        /// <param name="listNumbers">Список номеров для генерации QR-кодов</param>
        /// <param name="dataDictionary">Словарь с данными</param>
        public static void GenerateQrCodes(List<string> listNumbers, Dictionary<string, string> dataDictionary)
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

            foreach (var code in listNumbers)
            {
                switch (dataDictionary["Format"])
                {
                    case "BMP":
                        barcodeWriter.Write(code).Save(Data.PathQrCode + "\\" + code + ".bmp", ImageFormat.Bmp);
                        break;
                    case "PNG":
                        barcodeWriter.Write(code).Save(Data.PathQrCode + "\\" + code + ".png", ImageFormat.Png);
                        break;
                    case "JPEG":
                        barcodeWriter.Write(code).Save(Data.PathQrCode + "\\" + code + ".jpeg", ImageFormat.Jpeg);
                        break;
                    case "PNG и BMP":
                        barcodeWriter.Write(code).Save(Data.PathQrCode + "\\" + code + ".png", ImageFormat.Png);

                        encodingOptions.Width = int.Parse(dataDictionary["WidthBmp"]);
                        encodingOptions.Height = int.Parse(dataDictionary["HeightBmp"]);
                        barcodeWriter = new BarcodeWriter
                        {
                            Format = BarcodeFormat.QR_CODE,
                            Options = encodingOptions
                        };

                        barcodeWriter.Write(code).Save(Data.PathQrCode + code + ".bmp", ImageFormat.Bmp);
                        break;
                }
            }
            MessageBox.Show("QR-коды сгенерированы",
                            "Успешно!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
        #endregion

        #region Проверка полей и директории при генерации QR-кодов
        /// <summary>Проверка полей и директории при генерации QR-кодов</summary>
        /// <param name="number">Последний номер записанный в файле</param>
        /// <returns>Возвращает true, если все проверки прошли успешно, иначе false</returns>
        public static bool CheckingFieldsDirectoriesGeneratingQrCodes(string path, int indexFormat,string height, string width, string heightBmp, string widthBmp, out string number)
        {
            number = null;

            if (indexFormat == (int)Data.Formats.PngBmp)
            {
                if (string.IsNullOrEmpty(heightBmp) || string.IsNullOrEmpty(widthBmp))
                {
                    MessageBox.Show("Укажите размер изображения для QR-кода",
                                    "Некорректные данные",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(height) && string.IsNullOrEmpty(width))
            {
                MessageBox.Show("Укажите размер изображения для QR-кода",
                                "Некорректные данные",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            if (Checks.FileExist(path) && !Checks.EmptyFile(path))
            {
                MessageBox.Show("Файла по пути " + path + " не существует, либо он не имеет номеров для генерации QR-кодов",
                                "Ошибка при чтении файла",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            number = File.ReadLines(path).Last();

            if (!Checks.IsNumber(number))
            {
                MessageBox.Show("Файл содержит некорректные данные",
                                "Ошибка при считывании номера",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            OperationsFiles.CleaningDirectory();

            return true;
        }
        #endregion
    }
}
