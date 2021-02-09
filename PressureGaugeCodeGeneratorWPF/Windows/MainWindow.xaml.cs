namespace PressureGaugeCodeGenerator.Windows
{
    using PressureGaugeCodeGenerator.Classes;
    using PressureGaugeCodeGenerator.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
                    GetData.GetDepartment(ComboBoxDepartment), out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                TextBoxPath.Text = path;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
                TextBoxCountNumbers.Clear();
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
            Dictionary<string, string> settings = SaveAndReadSettings.ReadSettings();
            settings["Department"] = ComboBoxDepartment.SelectedIndex.ToString();
            settings["Checked"] = CheckBoxAutoSetYear.IsChecked.ToString();
            settings["Format"] = ComboBoxFormat.SelectedIndex.ToString();

            SaveAndReadSettings.SaveSettings(settings);
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex, TextBoxWidth.Text, TextBoxHeight.Text, TextBoxWidthBmp.Text,
                TextBoxHeightBmp.Text);
        }
        #endregion

        #region При загрузке окна
        /// <summary>При загрузке окна</summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> settings = SaveAndReadSettings.ReadSettings();

            if (settings.Count != 0)
            {
                CheckBoxAutoSetYear.IsChecked = bool.Parse(settings["Checked"]);
                ComboBoxDepartment.SelectedIndex = int.Parse(settings["Department"]);
                ComboBoxFormat.SelectedIndex = int.Parse(settings["Format"]);
            }
        }
        #endregion

        #region При изменении значения в "Формат изображения"
        /// <summary>При изменении значения в "Формат изображения"</summary>
        private void ComboBoxFormatSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFormat.SelectedIndex == 3)
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

            Dictionary<string, string> settings = SaveAndReadSettings.ReadSettings();

            switch (ComboBoxFormat.SelectedIndex)
            {
                //BMP
                case 0:
                    TextBoxWidth.Text = settings["BmpWidth"];
                    TextBoxHeight.Text = settings["BmpHeight"];
                    break;
                //PNG
                case 1:
                    TextBoxWidth.Text = settings["PngWidth"];
                    TextBoxHeight.Text = settings["PngHeight"];
                    break;
                //JPEG
                case 2:
                    TextBoxWidth.Text = settings["JpegWidth"];
                    TextBoxHeight.Text = settings["JpegHeight"];
                    break;
                //PNG и BMP
                case 3:
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
                    CheckBoxAutoSetYear.IsChecked = true;
                    break;
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
            MessageBox.Show(
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
            if (CheckingFieldsGeneratingNumbers())
            {
                OperationsFiles.Generate(int.Parse(TextBoxStartNumber.Text), int.Parse(TextBoxCountNumbers.Text),
                                TextBoxPath.Text, CheckBoxAutoSetYear.IsChecked, GetData.GetDepartment(ComboBoxDepartment));
                OperationsFiles.SetStartNumber(TextBoxPath.Text, CheckBoxAutoSetYear.IsChecked,
                    GetData.GetDepartment(ComboBoxDepartment), out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(TextBoxPath.Text);
            }
        }
        #endregion

        #region Валидация полей при генерации номеров
        /// <summary>Валидация полей при генерации номеров</summary>
        /// <returns>Возвращает true, если все поля заполнены верно, иначе false</returns>
        private bool CheckingFieldsGeneratingNumbers()
        {
            if (TextBoxStartNumber.Text.Length < GlobalVar.DIGITS)
            {
                MessageBox.Show($"В номере должно быть {GlobalVar.DIGITS} цифр", "Некорректный начальный номер!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!ChecksFile.IsNumber(TextBoxCountNumbers.Text))
            {
                MessageBox.Show("Введите корректное количество номеров", "Некорректное количество номеров!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!ChecksFile.IsNumber(TextBoxStartNumber.Text))
            {
                MessageBox.Show("Введите корректный начальный номер", "Некорректный начальный номер!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.Parse(TextBoxCountNumbers.Text) < 1 || int.Parse(TextBoxCountNumbers.Text) > 1000)
            {
                MessageBox.Show("Количество номеров должно быть в диапазоне от 1 до 1000",
                    "Некорректное количество номеров!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.Parse(TextBoxStartNumber.Text) + int.Parse(TextBoxCountNumbers.Text) >= 1_000_000_000)
            {
                MessageBox.Show("Последнее генерируемое число должно быть не более 999999999",
                    "Некорректное генерируемое число!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!ChecksFile.FileExist(TextBoxPath.Text))
            {
                MessageBox.Show("Введите корректный путь", "Некорректный путь!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (!ChecksFile.EmptyFile(TextBoxPath.Text))
            {
                if (!ChecksFile.IsNumber(File.ReadLines(TextBoxPath.Text).Last()))
                {
                    MessageBox.Show("В файле последний номер содержит недопустимые знаки",
                        "Некорректный последний номер в файле!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (!ChecksFile.ValidNumber(TextBoxPath.Text, TextBoxStartNumber.Text))
                {
                    MessageBox.Show("Номер должен быть больше, чем уже сгенерированное число в файле",
                        "Некорректный начальный номер!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region При клике на кнопку "Показать номера"
        /// <summary>При клике на кнопку "Показать номера"</summary>
        private void ButtonShowNumbers_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ChecksFile.CheckPath(TextBoxPath.Text))
                return;
            if (!ChecksFile.FileExist(TextBoxPath.Text))
                return;

            ListNumbersWindow numbersWindow = new ListNumbersWindow(TextBoxPath.Text);
            numbersWindow.Show();
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
                ? $"{Directory.GetCurrentDirectory()}\\numbers{GetData.GetDepartment(ComboBoxDepartment)}_20{GetData.GetYear()}.txt"
                : "";
        }
        #endregion

        #region При изменении текущего значения в ComboBox`е
        /// <summary>При изменении текущего значения в ComboBox`е</summary>
        private void ComboBoxDepartment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string path =
                $"{Directory.GetCurrentDirectory()}\\numbers{ComboBoxDepartment.SelectedItem.ToString().Remove(0, 38).Remove(1)}_20{GetData.GetYear()}.txt";

            if (ChecksFile.CheckFullPathAndFile(path))
            {
                OperationsFiles.SetStartNumber(TextBoxPath.Text = path, true, GetData.GetDepartment(ComboBoxDepartment),
                    out string startNumber);
                TextBoxStartNumber.Text = startNumber;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
                return;
            }

            if (ChecksFile.FileExist(path) && ChecksFile.EmptyFile(path))
            {
                TextBoxStartNumber.Text = $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment)}000001";
                TextBoxPath.Text = path;
                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
                return;
            }

            if (!ChecksFile.FileExist(path))
            {
                MessageBoxResult result = MessageBox.Show($"Файла по пути {path} не существует.\n" +
                                                          "Создать файл?", "Файла не существует",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        using (File.Create(path))
                        {
                            TextBoxStartNumber.Text =
                                $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment)}000001";
                            TextBoxPath.Text = path;
                            MessageBox.Show($"Создан файл {path}", "Оповещение о создании файла", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }

                LabelDrawNumbers.Content = OperationsFiles.DrawNumbers(path);
            }
            else
            {
                MessageBox.Show(
                    $"Файл по пути {path} содержит некорректные данные.\n" +
                    $"Файл должен содержать {GlobalVar.DIGITS}-значные номера",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                TextBoxStartNumber.Text = $"{GetData.GetYear()}{GetData.GetDepartment(ComboBoxDepartment)}000001";
            }
        }
        #endregion

        #region При клике на кнопку генерации QR-кода
        /// <summary>При клике на кнопку генерации QR-кода</summary>
        private void ButtonGenerateQr_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckingFieldsDirectoriesGeneratingQrCodes(out string number))
            {
                List<string> massNum = new List<string>();
                Dictionary<string, string> dataDictionary = new Dictionary<string, string>
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
                MessageBox.Show(
                    "QR-коды сгенерированы в каталог \"" + GlobalVar.NAME_FOLDER_QR,
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

            if (string.IsNullOrEmpty(TextBoxHeight.Text) && string.IsNullOrEmpty(TextBoxWidth.Text))
            {
                MessageBox.Show(
                    "Укажите размер изображения для QR-кода",
                    "Некорректные данные",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (ChecksFile.FileExist(TextBoxPath.Text) && ChecksFile.EmptyFile(TextBoxPath.Text))
            {
                MessageBox.Show(
                    "Файла по пути " + TextBoxPath.Text + " не существует, либо он не имеет номеров для генерации QR-кодов",
                    "Ошибка при чтении файла",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            number = File.ReadLines(TextBoxPath.Text).Last();

            if (!ChecksFile.IsNumber(number))
            {
                MessageBox.Show(
                    "Файл содержит некорректные данные",
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

            if (ComboBoxFormat.SelectedIndex == 3)
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

            if (string.IsNullOrEmpty(TextBoxHeight.Text) || string.IsNullOrEmpty(TextBoxWidth.Text))
            {
                MessageBox.Show(
                    "Укажите размер изображения для QR-кода",
                    "Некорректные данные",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
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

        #region При открытии раскрывающегося списка
        /// <summary>При открытии раскрывающегося списка</summary>
        private void ComboBoxFormat_DropDownOpened(object sender, EventArgs e)
        {
            SaveAndReadSettings.SaveSizesFormats(ComboBoxFormat.SelectedIndex, TextBoxWidth.Text, TextBoxHeight.Text,
                TextBoxWidthBmp.Text,
                TextBoxHeightBmp.Text);
        }
        #endregion
    }
}