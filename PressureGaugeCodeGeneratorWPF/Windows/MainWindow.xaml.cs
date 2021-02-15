namespace PressureGaugeCodeGenerator.Windows
{
    using CoreScanner;
    using PressureGaugeCodeGenerator.Classes;
    using PressureGaugeCodeGenerator.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainWindow : Window
    {
        private delegate void Create();
        static CCoreScannerClass cCoreScannerClass = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Create date = OperationsFiles.FileCreate;
            date += OperationsFiles.DirectoryCreate;
            date();

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
                cCoreScannerClass.ExecCommand(Data.SCAN_EVENT_СALL_СODE, Data.EVENTS, out _, out status);
                cCoreScannerClass.BarcodeEvent += OnBarcodeEvent;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Что-то не так, пожалуйста, проверьте... " + exp.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = SaveAndReadSettings.ReadSettings();
            settings["Department"] = ComboBoxDepartment.SelectedIndex.ToString();
            settings["CheckedYear"] = CheckBoxAutoSetYear.IsChecked.ToString();
            settings["CheckedStartNumber"] = CheckBoxCheckStartNumber.IsChecked.ToString();
            settings["Format"] = ComboBoxFormat.SelectedIndex.ToString();

            SaveAndReadSettings.SaveSettings(settings);
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex, TextBoxWidth.Text, TextBoxHeight.Text, TextBoxWidthBmp.Text, TextBoxHeightBmp.Text);
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void Open()
        {
            if (OperationsFiles.OpenFile(out string path))
            {
                OperationsFiles.SetStartNumber(path, CheckBoxAutoSetYear.IsChecked, Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()), out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                TextBoxPath.Text = path;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region Событие при сканирование QR-кода
        /// <summary>Событие при сканирование QR-кода</summary>
        void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            short[] scannerTypes = { 1 };
            short numberOfScannerTypes = 1;

            cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out _);
            OperationsQrCodes.DecodingQrCode(ref pscanData, out string decodedNumber);

            if (Checks.CheckingExistenceNumber(decodedNumber))
            {
                cCoreScannerClass.ExecCommand(Data.SCAN_ACTION_CODE, Data.BEEP_THREE_TIMES, out _, out _);
                cCoreScannerClass.ExecCommand(Data.SCAN_ACTION_CODE, Data.LED_RED, out _, out _);
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(Data.PathBaseNumbers, true))
                {
                    streamWriter.WriteLine(decodedNumber);
                }
                cCoreScannerClass.ExecCommand(Data.SCAN_ACTION_CODE, Data.BEEP_ONE_TIMES, out _, out _);
                cCoreScannerClass.ExecCommand(Data.SCAN_ACTION_CODE, Data.LED_GREEN, out _, out _);
            }
        }
        #endregion

        private void ComboBoxFormatSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFormat.SelectedIndex == (int)Data.Formats.PngBmp)
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
                case (int)Data.Formats.Bmp:
                    TextBoxWidth.Text = settings["BmpWidth"];
                    TextBoxHeight.Text = settings["BmpHeight"];
                    break;
                case (int)Data.Formats.Png:
                    TextBoxWidth.Text = settings["PngWidth"];
                    TextBoxHeight.Text = settings["PngHeight"];
                    break;
                case (int)Data.Formats.Jpeg:
                    TextBoxWidth.Text = settings["JpegWidth"];
                    TextBoxHeight.Text = settings["JpegHeight"];
                    break;
                case (int)Data.Formats.PngBmp:
                    TextBoxWidth.Text = settings["PngBmpWidthPng"];
                    TextBoxHeight.Text = settings["PngBmpHeightPng"];
                    TextBoxWidthBmp.Text = settings["PngBmpWidthBmp"];
                    TextBoxHeightBmp.Text = settings["PngBmpHeightBmp"];
                    break;
            }
        }

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

        private void MenuItemSupport_Click(object sender, RoutedEventArgs e)
        {
            Data.GetSupport();
        }

        private void MenuItemAboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutProgram aboutProgram = new AboutProgram();
            aboutProgram.ShowDialog();
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (Checks.CheckingFieldsGeneratingNumbers(TextBoxStartNumber.Text, TextBoxCountNumbers.Text, TextBoxPath.Text, CheckBoxCheckStartNumber.IsChecked))
            {
                OperationsFiles.Generate(int.Parse(TextBoxStartNumber.Text),
                                        int.Parse(TextBoxCountNumbers.Text),
                                        TextBoxPath.Text, CheckBoxAutoSetYear.IsChecked,
                                        Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()));
                OperationsFiles.SetStartNumber(TextBoxPath.Text,
                                               CheckBoxAutoSetYear.IsChecked,
                                               Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()),
                                               out string startNumber);

                TextBoxStartNumber.Text = startNumber;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(TextBoxPath.Text);
            }
        }

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

        private void CheckBoxAutoSetYear_OnChecked(object sender, RoutedEventArgs e)
        {
            TextBoxStartNumber.Text = TextBoxStartNumber.Text != ""
                                                              ? Data.GetYear() + TextBoxStartNumber.Text.Remove(0, 2)
                                                              : "";
            TextBoxPath.Text = TextBoxPath.Text != ""
                                                ? $"{Directory.GetCurrentDirectory()}\\numbers{Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}_20{Data.GetYear()}.txt"
                                                : "";
        }

        private void ComboBoxDepartment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\numbers{Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}_20{Data.GetYear()}.txt";

            if (Checks.CheckFullPathAndFile(path))
            {
                OperationsFiles.SetStartNumber(TextBoxPath.Text = path,
                                               true,
                                               Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString()),
                                               out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                return;
            }
            if (Checks.FileExist(path) && !Checks.EmptyFile(path))
            {
                TextBoxStartNumber.Text = $"{Data.GetYear()}{Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
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
                            TextBoxStartNumber.Text = $"{Data.GetYear()}{Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
                            TextBoxPath.Text = path;
                            MessageBox.Show($"Создан файл {path}", "Оповещение о создании файла", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show($"Файл по пути {path} содержит некорректные данные.\n" + $"Файл должен содержать {Data.DIGITS_IN_NUMBER}-значные номера",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                TextBoxStartNumber.Text = $"{Data.GetYear()}{Data.GetDepartment(ComboBoxDepartment.SelectedItem.ToString())}000001";
            }
        }

        private void ButtonGenerateQr_OnClick(object sender, RoutedEventArgs e)
        {
            if (OperationsQrCodes.CheckingFieldsDirectoriesGeneratingQrCodes(TextBoxPath.Text,
                                                                             ComboBoxFormat.SelectedIndex,
                                                                             TextBoxHeight.Text,
                                                                             TextBoxWidth.Text,
                                                                             TextBoxHeightBmp.Text,
                                                                             TextBoxWidthBmp.Text,
                                                                             out string number))
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

                OperationsQrCodes.GenerateQrCodes(massNum, dataDictionary);
            }
        }

        private void MenuItemOpen_OnClick(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void MenuItemGoNumbers_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $"file://{Directory.GetCurrentDirectory()}");
        }

        private void MenuItemGoQrCodes_OnClick(object sender, RoutedEventArgs e)
        {
            OperationsFiles.DirectoryCreate();
            Process.Start("explorer", $"file://{Data.PathQrCode}");
        }

        private void ComboBoxFormat_DropDownOpened(object sender, EventArgs e)
        {
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex,
                                                TextBoxWidth.Text,
                                                TextBoxHeight.Text,
                                                TextBoxWidthBmp.Text,
                                                TextBoxHeightBmp.Text);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1)
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(TextBoxPath.Text);
        }

        private void ButtonImposeQrCode_Click(object sender, RoutedEventArgs e)
        {
            ImageOverlayWindow imageOverlayWindow = new ImageOverlayWindow();
            imageOverlayWindow.ShowDialog();
        }
    }
}