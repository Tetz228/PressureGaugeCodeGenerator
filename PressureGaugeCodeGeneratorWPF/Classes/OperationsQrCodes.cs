namespace PressureGaugeCodeGenerator.Classes
{
    using System;
    using System.Xml;

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
    }
}
