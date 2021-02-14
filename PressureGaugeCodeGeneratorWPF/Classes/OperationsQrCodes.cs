namespace PressureGaugeCodeGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Xml;

    using PressureGaugeCodeGenerator.Data;

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
                        barcodeWriter.Write(code).Save(Data.NAME_FOLDER_QR + code + ".bmp", ImageFormat.Bmp);
                        break;
                    case "PNG":
                        barcodeWriter.Write(code).Save(Data.NAME_FOLDER_QR + code + ".png", ImageFormat.Png);
                        break;
                    case "JPEG":
                        barcodeWriter.Write(code).Save(Data.NAME_FOLDER_QR + code + ".jpeg", ImageFormat.Jpeg);
                        break;
                    case "PNG и BMP":
                        barcodeWriter.Write(code).Save(Data.NAME_FOLDER_QR + code + ".png", ImageFormat.Png);

                        encodingOptions.Width = int.Parse(dataDictionary["WidthBmp"]);
                        encodingOptions.Height = int.Parse(dataDictionary["HeightBmp"]);
                        barcodeWriter = new BarcodeWriter
                        {
                            Format = BarcodeFormat.QR_CODE,
                            Options = encodingOptions
                        };

                        barcodeWriter.Write(code).Save(Data.NAME_FOLDER_QR + code + ".bmp", ImageFormat.Bmp);
                        break;
                }
            }
        }
        #endregion
    }
}
