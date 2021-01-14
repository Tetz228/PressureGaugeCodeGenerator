using PressureGaugeCodeGenerator.Infrastructure;
using PressureGaugeCodeGenerator.Infrastructure.Commands;
using PressureGaugeCodeGenerator.Models;
using PressureGaugeCodeGenerator.ViewModels.Base;
using PressureGaugeCodeGenerator.Views.Windows;
using System.Collections.Generic;
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

        /// <summary>QR-коды</summary>
        public string QRСodes
        {
            get => _QRСodes;
            set => Set(ref _QRСodes, value);
        }

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

        #region SavingSettingsCloseWindowCommand

        public IDelegateCommand SavingSettingsCloseWindowCommand { get; set; }

        private void OnSavingSettingsCloseWindowCommandExecuted(object p)
        {
            
        }

        private bool CanSavingSettingsCloseWindowCommandExecuted(object p) => true;

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            OpenAboutProgramCommand = new LambdaCommand(OnOpenAboutProgramCommandExecuted, CanOpenAboutProgramCommandExecuted);
            OpenHelpWindowCommand = new LambdaCommand(OnOpenHelpWindowCommandExecuted, CanOpenHelpWindowCommandExecuted);
            //SavingSettingsCloseWindowCommand = new LambdaCommand(OnSavingSettingsCloseWindowCommandExecuted, CanSavingSettingsCloseWindowCommandExecuted);
            #endregion

            ListDepartments = new List<Departments>
            {
                new Departments{ NumberDepartment = 1 ,Name = "Литография (ППШ)"},
                new Departments{ NumberDepartment = 2 ,Name = "Безрегулировка"},
                new Departments{ NumberDepartment = 3 ,Name = "Безрегулировка (штучный циферблат)"},
                new Departments{ NumberDepartment = 4 ,Name = "ПНП"}
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
