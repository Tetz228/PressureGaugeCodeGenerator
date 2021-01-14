using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PressureGaugeCodeGenerator.Models
{
    internal class Manometers : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Номер манометра
        private int _NumberManometer;

        /// <summary>Номер манометра</summary>
        public int NumberManometer
        {
            get => _NumberManometer;
            set
            {
                _NumberManometer = value;
                OnPropertyChanged("NumberManometer");
            }
        }
        #endregion
    }
}
