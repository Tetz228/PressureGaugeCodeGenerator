namespace PressureGaugeCodeGenerator.Data
{
    using System;
    using System.IO;
    using System.Windows;

    public static class Data
    {
        public enum Formats
        {
            Bmp,
            Png,
            Jpeg,
            PngBmp
        }

        public static string GetYear() => DateTime.Now.ToString("yy");

        public static string GetDepartment(string department) => department.Remove(0, 38).Remove(1);

        public static void GetSupport() => MessageBox.Show(
                "Программа педназначена для генерации кодов манометров и QR-кодов.\n\n" +
                "В начале работы необходимо выбрать номер участка, для которого будет производиться генерация кодов.\n" +
                "В поле \"Начальный номер\" необходимо указать номер, с которого будет начинаться нумерация манометров. " +
                "Номер должен состоять из цифр и содержать 9 символов.\n" +
                "Первые две цифры программа устанавливают автоматически и они соответсвуют последним двум цифрам года. " +
                "Третья цифра соответствует номеру участка и устанавливается автоматически, в зависимости от выбранного участка. " +
                "Если номера для выбранного участка ранее сгенерированы не были, программа предложит начать генерацию с первого номера. " +
                "Можно указать начальный номер вручную в поле \"Начальный номер\".\n" +
                "Если номера были сгенерированы в программе ранее, они считаются из файла и " +
                "программа автоматически вставит в поле " +
                "\"Начальный номер\", следующий по порядку номер после последнего сгенерированного.\n\n" +
                "В поле \"Количество номеров\" нужно указать количество номеров, которые необходимо сгенерировать.\n" +
                "Количество номеров должно быть в диапазоне от 1-го до 1000.\n\n" +
                "В поле \"Путь к файлу\" программа автоматически подставляет файлы, соотвествующие номеру участка: \"numbers1_20ХХ.txt\", \"numbers2_20ХХ.txt\", \"numbers3_20ХХ.txt\", \"numbers4_20ХХ.txt\".\n" +
                "Путь к файлу можно указать вручную, нажав кнопку \"Открыть\" " +
                "и выбрать в появившемся диалоговом окне, необходимый файл. " +
                "Файлы должны иметь расширение \"*.txt\".\n\n" +
                "После заполнения всех обязательных полей, необходимо нажать кнопку \"Генерировать\". " +
                "Произойдет генерация номеров в указанный файл, о чём программа выдаст сообщение.\n" +
                "Кнопка \"Показать номера\", позволяет просмотреть номера, сгенерированные в файле.\n" +
                "Если необходимо отключить автоматическую генерацию года в начале номера, необходимо снять галочку \"Установить года автоматически\" " +
                "Чтобы создать QR-код из сгенерированных номеров, нужно перейти на вкладку \"QR-код\", выбрать формат файла и указать разрешение, затем нажать кнопку \"Генерировать QR\".\n" +
                "Сгенерированные изображения QR-кодов будут находиться в каталоге \"qrcode\".\n" +
                "После этого программу можно закрыть, нажав кнопку \"Выход\".",
                "Справка",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

        public const int DIGITS_IN_NUMBER = 9;
        public static string PathQrCode = $"{Directory.GetCurrentDirectory()}\\QR-codes";
        public static string PathBaseNumbers = $"{Directory.GetCurrentDirectory()}\\BaseNumbers";
        public const int SCAN_EVENT_СALL_СODE = 1001;
        public const string EVENTS = "<inArgs>" +
                                             "<cmdArgs>" +
                                             "<arg-int>1</arg-int>" +
                                             "<arg-int>1</arg-int>" +
                                             "</cmdArgs>" +
                                             "</inArgs>";
        public const int SCAN_ACTION_CODE = 6000;
        public const string BEEP_THREE_TIMES = "<inArgs>" +
                                             "<scannerID>1</scannerID>" +
                                             "<cmdArgs>" +
                                             "<arg-int>3</arg-int>" +
                                             "</cmdArgs>" +
                                             "</inArgs>";
        public const string BEEP_ONE_TIMES = "<inArgs>" +
                                           "<scannerID>1</scannerID>" +
                                           "<cmdArgs>" +
                                           "<arg-int>0</arg-int>" +
                                           "</cmdArgs>" +
                                           "</inArgs>";
        public const string LED_GREEN = "<inArgs>" +
                                       "<scannerID>1</scannerID>" +
                                       "<cmdArgs>" +
                                       "<arg-int>43</arg-int>" +
                                       "</cmdArgs>" +
                                       "</inArgs>";
        public const string LED_RED = "<inArgs>" +
                                     "<scannerID>1</scannerID>" +
                                     "<cmdArgs>" +
                                     "<arg-int>47</arg-int>" +
                                     "</cmdArgs>" +
                                     "</inArgs>";
    }
}
