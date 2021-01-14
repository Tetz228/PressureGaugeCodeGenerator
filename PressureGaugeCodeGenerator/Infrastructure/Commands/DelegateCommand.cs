using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureGaugeCodeGenerator.Infrastructure
{
    class DelegateCommand : IDelegateCommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public DelegateCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        public DelegateCommand(Action<object> Execute)
        {
            _Execute = Execute;
            _CanExecute = AlwaysCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            Execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
    }
}
