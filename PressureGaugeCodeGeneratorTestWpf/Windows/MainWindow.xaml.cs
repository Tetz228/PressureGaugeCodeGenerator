﻿using PressureGaugeCodeGeneratorTestWpf.Commands;
using PressureGaugeCodeGeneratorTestWpf.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PressureGaugeCodeGeneratorTestWpf.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region При клике на кнопку в меню "Открыть"
        /// <summary>При клике на кнопку в меню "Открыть"</summary>
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }
        #endregion

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
                OperationsFiles.SetStartNumber((path, CheckBoxAutoSetYear.IsChecked, ComboBoxDepartment.Text), out string startNumber);
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
            Dictionary<string, string> settings = new Dictionary<string, string>
            {
                { "Department", ComboBoxDepartment.Text },
                { "Width", TextBoxWidth.Text },
                { "Height", TextBoxHeight.Text },
                { "Width_BMP", TextBoxWidthBmp.Text },
                { "Height_BMP", TextBoxHeightBmp.Text },
                { "Checked", CheckBoxAutoSetYear.IsChecked.ToString() },
                { "Format", ComboBoxFormat.Text }
            };
            Settings.SaveSettings(settings);
        }
        #endregion

        #region При загрузке окна
        /// <summary>При загрузке окна</summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> settings = Settings.ReadSettings();

            if (settings.Count != 0)
            {
                TextBoxWidth.Text = settings["Width"];
                TextBoxHeight.Text = settings["Height"];
                TextBoxWidthBmp.Text = settings["Width_BMP"];
                TextBoxHeightBmp.Text = settings["Height_BMP"];
                CheckBoxAutoSetYear.IsChecked = bool.Parse(settings["Checked"]);
                switch (settings["Department"])
                {
                    case "1 - Литография (ППШ)":
                        ComboBoxDepartment.SelectedIndex = 0;
                        break;
                    case "2 - Безрегулировка":
                        ComboBoxDepartment.SelectedIndex = 1;
                        break;
                    case "3 - Безрегулировка (штучный циферблат)":
                        ComboBoxDepartment.SelectedIndex = 2;
                        break;
                    case "4 - ПНП":
                        ComboBoxDepartment.SelectedIndex = 3;
                        break;
                }
                switch (settings["Format"])
                {
                    case "BMP":
                        ComboBoxFormat.SelectedIndex = 0;
                        break;
                    case "PNG":
                        ComboBoxFormat.SelectedIndex = 1;
                        break;
                    case "JPEG":
                        ComboBoxFormat.SelectedIndex = 2;
                        break;
                    case "PNG и BMP":
                        ComboBoxFormat.SelectedIndex = 3;
                        break;
                }
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
            if (TextBoxStartNumber.Text.Length < GlobalVar.DIGITS)
            {
                MessageBox.Show($"В номере должно быть {GlobalVar.DIGITS} цифр", "Некорректный начальный номер!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(TextBoxCountNumbers.Text, out _))
            {
                MessageBox.Show("Введите корректное количество номеров", "Некорректное количество номеров!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(TextBoxStartNumber.Text, out _))
            {
                MessageBox.Show("Введите корректный начальный номер", "Некорректный начальный номер!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.Parse(TextBoxCountNumbers.Text) < 1 || int.Parse(TextBoxCountNumbers.Text) > 1000)
            {
                MessageBox.Show("Количество номеров должно быть в диапазоне от 1 до 1000", "Некорректное количество номеров!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.Parse(TextBoxStartNumber.Text) + int.Parse(TextBoxCountNumbers.Text) >= 1_000_000_000)
            {
                MessageBox.Show("Последнее генерируемое число должно быть не более 999999999", "Некорректное генерируемое число!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!OperationsFiles.FileExist(TextBoxPath.Text))
            {
                MessageBox.Show("Введите корректный путь", "Некорректный путь!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!OperationsFiles.EmptyFile(TextBoxPath.Text))
            {
                if (!OperationsFiles.IsNumber(File.ReadLines(TextBoxPath.Text).Last()))
                {
                    MessageBox.Show("В файле последний номер содержит недопустимые знаки", "Некорректный последний номер в файле!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!OperationsFiles.ValidNumber(TextBoxPath.Text, TextBoxStartNumber.Text))
                {
                    MessageBox.Show("Номер должен быть больше, чем уже сгенерированное число в файле", "Некорректный начальный номер!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //OperationsFiles.Generate(TextBoxStartNumber.Text, TextBoxCountNumbers.Text, TextBoxPath.Text, GlobalVar.DIGITS);
        }

        #endregion

        #region При клике на кнопку "Показать номера"
        /// <summary>При клике на кнопку "Показать номера"</summary>
        private void ButtonShowNumbers_OnClick(object sender, RoutedEventArgs e)
        {
            OperationsFiles.ReadingFileNumbers(TextBoxPath.Text);
        }
        #endregion

        #region При включении автоматической установки года
        /// <summary>При включении автоматической установки года</summary>
        private void CheckBoxAutoSetYear_OnChecked(object sender, RoutedEventArgs e)
        {
            TextBoxStartNumber.Text = OperationsFiles.GetYear() + TextBoxStartNumber.Text.Remove(0, 2);
            TextBoxPath.Text = $"{Directory.GetCurrentDirectory()}\\numbers{ComboBoxDepartment.Text}_20{OperationsFiles.GetYear()}.txt";
        }
        #endregion

        private void ComboBoxDepartment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxPath.Text = OperationsFiles.Handle(ComboBoxDepartment.Text);
        }
    }
}

