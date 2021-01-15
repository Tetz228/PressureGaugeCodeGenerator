using PressureGaugeCodeGenerator.Infrastructure;
using PressureGaugeCodeGenerator.Infrastructure.Commands;
using PressureGaugeCodeGenerator.Models;
using PressureGaugeCodeGenerator.ViewModels.Base;
using PressureGaugeCodeGenerator.Views.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PressureGaugeCodeGenerator.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        #region Заголовок окна

        private string _Title = "Генератор кодов манометров";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Путь к файлу

        private string _PathToFile = "Путь к файлу:";

        /// <summary>Путь к файлу</summary>
        public string PathToFile
        {
            get => _PathToFile;
            set => Set(ref _PathToFile, value);
        }

        #endregion

        #region Номера

        private string _Number = "Номера";

        /// <summary>Номера</summary>
        public string Numbers
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        #region Участок

        private string _Department = "Участок";

        /// <summary>Участок</summary>
        public string Department
        {
            get => _Department;
            set => Set(ref _Department, value);
        }

        public List<Departments> ListDepartments { get; set; }

        #endregion

        #region Начальный номер

        private string _StartNumber = "Начальный номер";

        /// <summary>Начальный номер</summary>
        public string StartNumber
        {
            get => _StartNumber;
            set => Set(ref _StartNumber, value);
        }

        #endregion

        #region Количество номеров

        private string _CountNumbers = "Количество номеров";

        /// <summary>Количество номеров</summary>
        public string CountNumbers
        {
            get => _CountNumbers;
            set => Set(ref _CountNumbers, value);
        }

        #endregion
        #endregion

        #region QR-коды

        private string _QRСodes = "QR-коды";

        /// <summary>QR-коды</summary> 42342
        public string QRСodes
        {
            get => _QRСodes;
            set => Set(ref _QRСodes, value);
        }

        //private string _QRСodes = "QR-коды";

        ///// <summary>QR-коды</summary>
        //public string QRСodes
        //{
        //    get => _QRСodes;
        //    set => Set(ref _QRСodes, value);
        //}

        public List<Qr_codes> ListQr_codes { get; set; }

        #endregion

        #region Команды
        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecuted(object p) => true;
        #endregion

        #region OpenAboutProgramCommand
        public ICommand OpenAboutProgramCommand { get; }

        private void OnOpenAboutProgramCommandExecuted(object p)
        {
            AboutProgram aboutProgram = new AboutProgram();
            aboutProgram.ShowDialog();
        }

        private bool CanOpenAboutProgramCommandExecuted(object p) => true;
        #endregion

        #region OpenHelpWindowCommand
        public ICommand OpenHelpWindowCommand { get; }

        private void OnOpenHelpWindowCommandExecuted(object p)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

        private bool CanOpenHelpWindowCommandExecuted(object p) => true;
        #endregion    
        #endregion

        #region При закрытии главного окна
        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //SaveSettings("department", (int.Parse(_department) - 1).ToString());
            //SaveSettings("format", _format);
            //SaveSettings("width", _width);
            //SaveSettings("height", _height);
            //SaveSettings("width_bmp", _width_bmp);
            //SaveSettings("height_bmp", _height_bmp);
            //if (checkBox_setYear.IsChecked == true)
            //{
            //    SaveSettings("checked", "true");
            //}
            //else
            //{
            //    SaveSettings("checked", "false");
            //}

            //private void SaveSettings(string _key, string _value)
            //{
            //    try
            //    {
            //        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //        var settings = configFile.AppSettings.Settings;
            //        if (settings[_key] == null)
            //        {
            //            settings.Add(_key, _value);
            //        }
            //        else
            //        {
            //            settings[_key].Value = _value;
            //        }
            //        configFile.Save(ConfigurationSaveMode.Modified);
            //        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            //    }
            //    catch (ConfigurationErrorsException ex)
            //    {
            //        MessageBox.Show(
            //            ex.Message,
            //            "Ошибка",
            //            MessageBoxButton.OK,
            //            MessageBoxImage.Error);
            //    }
            //}
        }
        #endregion

        #region При загрузки окна главного окна
        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ReadSettings();
        }

        /// <summary>Чтение настроек</summary>
        //private void ReadSettings()
        //{
        //    try
        //    {
        //        var appSettings = ConfigurationManager.AppSettings;

        //        if (appSettings.Count == 0)
        //        {
        //            MessageBox.Show(
        //                "Ошибка чтения настроек\n" +
        //                "Настройки не найдены",
        //                "Ошибка",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            //    int width = int.Parse(appSettings["width"]);
        //            //    int height = int.Parse(appSettings["height"]);
        //            string width = appSettings["width"];
        //            string height = appSettings["height"];
        //            string width_bmp = appSettings["width_bmp"];
        //            string height_bmp = appSettings["height_bmp"];
        //            string department = appSettings["department"];
        //            string format = appSettings["format"];
        //            string ischecked = appSettings["checked"];

        //            //       Application.Current.MainWindow.Width = width;
        //            //       Application.Current.MainWindow.Height = height;
        //            textBox_width.Text = width;
        //            textBox_height.Text = height;
        //            textBox_width_bmp.Text = width_bmp;
        //            textBox_height_bmp.Text = height_bmp;
        //            comboBox_department.SelectedIndex = int.Parse(department);
        //            setFormat(format);
        //            switch (ischecked)
        //            {
        //                case "true":
        //                    checkBox_setYear.IsChecked = true;
        //                    break;
        //                case "false":
        //                    checkBox_setYear.IsChecked = false;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (ConfigurationErrorsException ex)
        //    {
        //        MessageBox.Show(
        //                ex.Message,
        //                "Ошибка",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Error);
        //    }
        //}
        #endregion

        #region Проверка пути файла
        /// <summary>Проверка пути файла</summary>
        public bool CheckPath(string _path)
        {

            string p = System.IO.Path.GetDirectoryName(_path);
            if (Directory.Exists(System.IO.Path.GetDirectoryName(_path)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            OpenAboutProgramCommand = new LambdaCommand(OnOpenAboutProgramCommandExecuted, CanOpenAboutProgramCommandExecuted);
            OpenHelpWindowCommand = new LambdaCommand(OnOpenHelpWindowCommandExecuted, CanOpenHelpWindowCommandExecuted);          
            #endregion

            ListDepartments = new List<Departments>
            {
                new Departments{ Name = "1 - Литография (ППШ)"},
                new Departments{ Name = "2 - Безрегулировка"},
                new Departments{ Name = "3 - Безрегулировка (штучный циферблат)"},
                new Departments{ Name = "4 - ПНП"}
            };
            ListQr_codes = new List<Qr_codes>
            {
                new Qr_codes{ Format = "BMP"},
                new Qr_codes{ Format = "PNG"},
                new Qr_codes{ Format = "JPEG"},
                new Qr_codes{ Format = "BMP + PNG"}
            };
        }
    }
}
