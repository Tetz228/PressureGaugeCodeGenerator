using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PressureGaugeCodeGenerator.Models
{
    internal class Manometers: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Номер участка
        private int _Number;

        /// <summary>Номер участка</summary>
        public int Number
        {
            get => _Number;
            set
            {
                _Number = value;
                OnPropertyChanged("Number");
            }
        }
        #endregion
    }
}
