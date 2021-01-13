using PressureGaugeCodeGenerator.Infrastructure.Commands;
using PressureGaugeCodeGenerator.ViewModels.Base;
using PressureGaugeCodeGenerator.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PressureGaugeCodeGenerator.ViewModels
{
    class MainWindowViewModel: ViewModel
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

        #region Номера

        private string _Number = "     Номера     ";

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

        #region Путь к файлу

        private string _PathToFile = "Путь к файлу";

        /// <summary>Путь к файлу</summary>
        public string PathToFile
        {
            get => _PathToFile;
            set => Set(ref _PathToFile, value);
        }

        #endregion

        #endregion

        #region QR-коды

        private string _QRСodes = "     QR-коды     ";

        /// <summary>QR-коды</summary>
        public string QRСodes
        {
            get => _QRСodes;
            set => Set(ref _QRСodes, value);
        }

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

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new CheckCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            OpenAboutProgramCommand = new CheckCommand(OnOpenAboutProgramCommandExecuted, CanOpenAboutProgramCommandExecuted);
            OpenHelpWindowCommand = new CheckCommand(OnOpenHelpWindowCommandExecuted, CanOpenHelpWindowCommandExecuted);
            #endregion
        }
    }
}
