using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PressureGaugeCodeGenerator.Models
{
    internal class Qr_codes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Высота
        private int _Height;

        /// <summary>Высота</summary>
        public int Height
        {
            get => _Height;
            set
            {
                _Height = value;
                OnPropertyChanged("Height");
            }
        }
        #endregion

        #region Ширина
        private int _Width;

        /// <summary>Ширина</summary>
        public int Width
        {
            get => _Width;
            set
            {
                _Width = value;
                OnPropertyChanged("Width");
            }
        }
        #endregion

        #region Формат для сохранения
        private string _Format;

        /// <summary>Формат для сохранения</summary>
        public string Format
        {
            get => _Format;
            set
            {
                _Format = value;
                OnPropertyChanged("Format");
            }
        }
        #endregion
    }
}
