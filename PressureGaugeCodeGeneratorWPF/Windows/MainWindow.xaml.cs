namespace PressureGaugeCodeGenerator.Windows
{
    using CoreScanner;
    using PressureGaugeCodeGenerator.Classes;
    using PressureGaugeCodeGenerator.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Xml;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.OleDb;

    public partial class MainWindow : Window
    {
        static CCoreScannerClass cCoreScannerClass = new CCoreScannerClass();
        public static string connectString = ConfigurationManager.ConnectionStrings["PressureGaugeCodeGenerator.Properties.Settings.BaseNumberConnectionString"].ConnectionString;

        private OleDbConnection myConnection;

        public MainWindow()
        {
            InitializeComponent();

            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }

        #region При клике на кнопку "Открыть"
        /// <summary>При клике на кнопку "Открыть"</summary>
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }
        #endregion

        #region Вызов данного метода при нажатии на кнопки "Открыть"
        /// <summary>Вызов данного метода при нажатии на кнопки "Открыть"</summary>
        private void Open()
        {
            if (OperationsFiles.OpenFile(out string path))
            {
                OperationsFiles.SetStartNumber(path, CheckBoxAutoSetYear.IsChecked,
                    GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()), out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                TextBoxPath.Text = path;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
            }
        }
        #endregion

        #region При клике на кнопку в меню "Выход"
        /// <summary>При клике на кнопку в меню "Выход"</summary>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region При закрытии окна
        /// <summary>При закрытии окна</summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            myConnection.Close();

            var settings = SaveAndReadSettings.ReadSettings();
            settings["Department"] = ComboBoxDepartment.SelectedIndex.ToString();
            settings["CheckedYear"] = CheckBoxAutoSetYear.IsChecked.ToString();
            settings["CheckedStartNumber"] = CheckBoxCheckStartNumber.IsChecked.ToString();
            settings["Format"] = ComboBoxFormat.SelectedIndex.ToString();

            SaveAndReadSettings.SaveSettings(settings);
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex, TextBoxWidth.Text, TextBoxHeight.Text, TextBoxWidthBmp.Text, TextBoxHeightBmp.Text);
        }
        #endregion

        #region При загрузке окна
        /// <summary>При загрузке окна</summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = SaveAndReadSettings.ReadSettings();

            if (settings.Count != 0)
            {
                CheckBoxAutoSetYear.IsChecked = bool.Parse(settings["CheckedYear"]);
                CheckBoxCheckStartNumber.IsChecked = bool.Parse(settings["CheckedStartNumber"]);
                ComboBoxDepartment.SelectedIndex = int.Parse(settings["Department"]);
                ComboBoxFormat.SelectedIndex = int.Parse(settings["Format"]);
            }

            try
            {
                short[] scannerTypes = { 1 };
                short numberOfScannerTypes = 1;

                cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out int status);

                int opcode = 1001;
                string inXML = "<inArgs>" +
                               "<cmdArgs>" +
                               "<arg-int>1</arg-int>" +
                               "<arg-int>1</arg-int>" +
                               "</cmdArgs>" +
                               "</inArgs>";

                cCoreScannerClass.ExecCommand(opcode, ref inXML, out string outXML, out status);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Что-то не так, пожалуйста, проверьте... " + exp.Message);
            }

            cCoreScannerClass.BarcodeEvent += OnBarcodeEvent;
        }
        #endregion

        #region Событие при сканирование QR-кода
        /// <summary>Событие при сканирование QR-кода</summary>
        void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(pscanData);

            string decoder = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
            string[] numbers = decoder.Split(' ');
            string strData = "";

            foreach (string number in numbers)
            {
                if (string.IsNullOrEmpty(number))
                    break;

                strData += ((char)Convert.ToInt32(number, 16)).ToString();
            }

            short[] scannerTypes = { 1 };
            short numberOfScannerTypes = 1;
            int opcode = 6000, isLine = 0;
            string inXML;

            cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out int status);

            StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\test.txt");
            string line = sr.ReadLine();

            while (line != null)
            {
                if (line == strData)
                    isLine++;

                line = sr.ReadLine();
            }

            if (isLine >= 1)
                inXML = "<inArgs>" +
                        "<scannerID>1</scannerID>" +
                        "<cmdArgs>" +
                        "<arg-int>3</arg-int>" +
                        "</cmdArgs>" +
                        "</inArgs>";
            else
                inXML = "<inArgs>" +
                        "<scannerID>1</scannerID>" +
                        "<cmdArgs>" +
                        "<arg-int>0</arg-int>" +
                        "</cmdArgs>" +
                        "</inArgs>";

            cCoreScannerClass.ExecCommand(opcode, ref inXML, out string outXML, out status);
        }
        #endregion

        #region При изменении значения в "Формат изображения"
        /// <summary>При изменении значения в "Формат изображения"</summary>
        private void ComboBoxFormatSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFormat.SelectedIndex == (int)GetData.Formats.PngBmp)
            {
                label_bmp.Visibility = Visibility.Visible;
                TextBoxHeightBmp.Visibility = Visibility.Visible;
                TextBoxWidthBmp.Visibility = Visibility.Visible;
            }
            else
            {
                label_bmp.Visibility = Visibility.Hidden;
                TextBoxHeightBmp.Visibility = Visibility.Hidden;
                TextBoxWidthBmp.Visibility = Visibility.Hidden;
            }

            var settings = SaveAndReadSettings.ReadSettings();

            switch (ComboBoxFormat.SelectedIndex)
            {
                case (int)GetData.Formats.Bmp:
                    TextBoxWidth.Text = settings["BmpWidth"];
                    TextBoxHeight.Text = settings["BmpHeight"];
                    break;
                case (int)GetData.Formats.Png:
                    TextBoxWidth.Text = settings["PngWidth"];
                    TextBoxHeight.Text = settings["PngHeight"];
                    break;
                case (int)GetData.Formats.Jpeg:
                    TextBoxWidth.Text = settings["JpegWidth"];
                    TextBoxHeight.Text = settings["JpegHeight"];
                    break;
                case (int)GetData.Formats.PngBmp:
                    TextBoxWidth.Text = settings["PngBmpWidthPng"];
                    TextBoxHeight.Text = settings["PngBmpHeightPng"];
                    TextBoxWidthBmp.Text = settings["PngBmpWidthBmp"];
                    TextBoxHeightBmp.Text = settings["PngBmpHeightBmp"];
                    break;
            }
        }
        #endregion

        #region При выключении автоматической установки года
        /// <summary>При выключении автоматической установки года</summary>
        private void CheckBoxAutoSetYear_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отключить автоматическую установку года?",
                                                      "Автоматическая установка года",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CheckBoxAutoSetYear.IsChecked = false;
                    break;
                case MessageBoxResult.No:
                case MessageBoxResult.Cancel:
                    CheckBoxAutoSetYear.IsChecked = true;
                    break;
            }
        }
        #endregion

        #region Открытие окна "Поддержка"
        /// <summary>Открытие окна "Поддержка"</summary>
        private void MenuItemSupport_Click(object sender, RoutedEventArgs e)
        {
            GetData.GetSupport();
        }
        #endregion

        #region Открытие окна "О программе"
        /// <summary>Открытие окна "О программе"</summary>
        private void MenuItemAboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutProgram aboutProgram = new AboutProgram();
            aboutProgram.ShowDialog();
        }
        #endregion

        #region При клике на кнопку "Сгенерировать"
        /// <summary>При клике на кнопку "Сгенерировать"</summary>
        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (Checks.CheckingFieldsGeneratingNumbers(TextBoxStartNumber.Text, TextBoxCountNumbers.Text, TextBoxPath.Text, CheckBoxAutoSetYear.IsChecked))
            {
                OperationsFiles.Generate(int.Parse(TextBoxStartNumber.Text),
                                        int.Parse(TextBoxCountNumbers.Text),
                                        TextBoxPath.Text, CheckBoxAutoSetYear.IsChecked,
                                        GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()));
                OperationsFiles.SetStartNumber(TextBoxPath.Text,
                                               CheckBoxAutoSetYear.IsChecked,
                                               GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()),
                                               out string startNumber);

                TextBoxStartNumber.Text = startNumber;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(TextBoxPath.Text);
            }
        }
        #endregion

        #region При клике на кнопку "Показать номера"
        /// <summary>При клике на кнопку "Показать номера"</summary>
        private void ButtonShowNumbers_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Checks.FileExist(TextBoxPath.Text))
            {
                MessageBox.Show("Некорректный путь или файл отсутствует", "Ошибка!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            ListNumbersWindow numbersWindow = new ListNumbersWindow(TextBoxPath.Text);
            numbersWindow.ShowDialog();
        }
        #endregion

        #region При включении автоматической установки года
        /// <summary>При включении автоматической установки года</summary>
        private void CheckBoxAutoSetYear_OnChecked(object sender, RoutedEventArgs e)
        {
            TextBoxStartNumber.Text = TextBoxStartNumber.Text != ""
                                                              ? GetData.GetYear() + TextBoxStartNumber.Text.Remove(0, 2)
                                                              : "";
            TextBoxPath.Text = TextBoxPath.Text != ""
                                                ? $"{Directory.GetCurrentDirectory()}\\numbers{GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}_20{GetData.GetYear()}.txt"
                                                : "";
        }
        #endregion

        #region При изменении текущего значения в ComboBox`е
        /// <summary>При изменении текущего значения в ComboBox`е</summary>
        private void ComboBoxDepartment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\numbers{GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}_20{GetData.GetYear()}.txt";

            if (Checks.CheckFullPathAndFile(path))
            {
                OperationsFiles.SetStartNumber(TextBoxPath.Text = path,
                                               true,
                                               GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()),
                                               out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                return;
            }
            if (Checks.FileExist(path) && Checks.EmptyFile(path))
            {
                TextBoxStartNumber.Text = $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
                TextBoxPath.Text = path;
                return;
            }
            if (!Checks.FileExist(path))
            {
                MessageBoxResult result = MessageBox.Show($"Файла по пути {path} не существует.\n" + "Создать файл?",
                                                          "Файла не существует",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Error);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        using (File.Create(path))
                        {
                            TextBoxStartNumber.Text = $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
                            TextBoxPath.Text = path;
                            MessageBox.Show($"Создан файл {path}", "Оповещение о создании файла", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show($"Файл по пути {path} содержит некорректные данные.\n" + $"Файл должен содержать {GlobalVar.DIGITS}-значные номера",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                TextBoxStartNumber.Text = $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
            }
        }
        #endregion

        #region При клике на кнопку генерации QR-кода
        /// <summary>При клике на кнопку генерации QR-кода</summary>
        private void ButtonGenerateQr_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckingFieldsDirectoriesGeneratingQrCodes(out string number))
            {
                var massNum = new List<string>();
                var dataDictionary = new Dictionary<string, string>
                {
                    {"Format",ComboBoxFormat.Text },
                    {"Width",TextBoxWidth.Text },
                    {"Height",TextBoxHeight.Text },
                    {"WidthBmp",TextBoxWidthBmp.Text },
                    {"HeightBmp",TextBoxHeightBmp.Text }
                };

                using (var streamReader = new StreamReader(TextBoxPath.Text))
                {
                    for (number = streamReader.ReadLine(); number != null; number = streamReader.ReadLine())
                        massNum.Add(number);
                }

                OperationsFiles.GenerateQrCodes(massNum, dataDictionary);
                MessageBox.Show("QR-коды сгенерированы в каталог \"" + GlobalVar.NAME_FOLDER_QR,
                                "Успешно!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
        #endregion

        #region Проверка полей и директории при генерации QR-кодов
        /// <summary>Проверка полей и директории при генерации QR-кодов</summary>
        /// <param name="number">Последний номер записанный в файле</param>
        /// <returns>Возвращает true, если все проверки прошли успешно, иначе false</returns>
        private bool CheckingFieldsDirectoriesGeneratingQrCodes(out string number)
        {
            number = null;

            if (ComboBoxFormat.SelectedIndex == (int)GetData.Formats.PngBmp)
            {
                if (string.IsNullOrEmpty(TextBoxHeightBmp.Text) || string.IsNullOrEmpty(TextBoxWidthBmp.Text))
                {
                    MessageBox.Show(
                        "Укажите размер изображения для QR-кода",
                        "Некорректные данные",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(TextBoxHeight.Text) && string.IsNullOrEmpty(TextBoxWidth.Text))
            {
                MessageBox.Show("Укажите размер изображения для QR-кода",
                                "Некорректные данные",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            if (Checks.FileExist(TextBoxPath.Text) && Checks.EmptyFile(TextBoxPath.Text))
            {
                MessageBox.Show("Файла по пути " + TextBoxPath.Text + " не существует, либо он не имеет номеров для генерации QR-кодов",
                                "Ошибка при чтении файла",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            number = File.ReadLines(TextBoxPath.Text).Last();

            if (!Checks.IsNumber(number))
            {
                MessageBox.Show("Файл содержит некорректные данные",
                                "Ошибка при считывании номера",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            if (Directory.Exists(GlobalVar.NAME_FOLDER_QR))
            {
                string[] filesInFolder = Directory.GetFiles(GlobalVar.NAME_FOLDER_QR, "*.*");
                foreach (string f in filesInFolder)
                    File.Delete(f);
            }
            else
                Directory.CreateDirectory(GlobalVar.NAME_FOLDER_QR);

            return true;
        }
        #endregion

        #region При клике на кнопку в меню "Открыть"
        /// <summary>При клике на кнопку в меню "Открыть"</summary>
        private void MenuItemOpen_OnClick(object sender, RoutedEventArgs e)
        {
            Open();
        }
        #endregion

        #region При клике на "Перейти к номерам"
        /// <summary>При клике на "Перейти к номерам"</summary>
        private void MenuItemGoNumbers_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $"file://{Path.GetDirectoryName(TextBoxPath.Text)}");
        }
        #endregion

        #region При клике на "Перейти к QR-кодам"
        /// <summary>При клике на "Перейти к QR-кодам"</summary>
        private void MenuItemGoQrCodes_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $"file://{Path.GetDirectoryName(TextBoxPath.Text)}\\" + GlobalVar.NAME_FOLDER_QR);
        }
        #endregion

        #region При клике на "Наложить QR-код"
        /// <summary>При клике на "Наложить QR-код"</summary>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ImageOverlayWindow imageOverlayWindow = new ImageOverlayWindow();
            imageOverlayWindow.ShowDialog();
        }
        #endregion

        #region При открытии ComboBox`а
        /// <summary>При открытии ComboBox`a</summary>
        private void ComboBoxFormat_DropDownOpened(object sender, EventArgs e)
        {
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex,
                                                TextBoxWidth.Text,
                                                TextBoxHeight.Text,
                                                TextBoxWidthBmp.Text,
                                                TextBoxHeightBmp.Text);
        }
        #endregion

        #region При изменении значения в TabControl
        /// <summary>При изменении значения в TabControl</summary>
        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1)
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(TextBoxPath.Text);
        }
        #endregion
    }
}