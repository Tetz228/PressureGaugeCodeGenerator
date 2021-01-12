using PressureGaugeCodeGenerator.ViewModels.Base;
using PressureGaugeCodeGenerator.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PressureGaugeCodeGenerator.ViewModels
{
    class HelpWindowViewModel: ViewModel
    {
        #region Заголовок окна

        private string _Title = "Поддержка";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Команды

        #region CloseApplicationCommand

        public ICommand CloseWindowCommand { get; }

        private void OnCloseWindowCommandExecuted(object p)
        {
            
        }

        private bool CanCloseWindowCommandExecuted(object p) => true;

        #endregion

        #endregion
    }
}
