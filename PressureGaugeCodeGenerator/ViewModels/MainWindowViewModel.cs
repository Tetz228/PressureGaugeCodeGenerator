﻿using PressureGaugeCodeGenerator.Infrastructure.Commands;
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
