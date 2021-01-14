using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PressureGaugeCodeGenerator.Models
{
    internal class Departments : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Номер участка
        private int _NumberDepartment;

        /// <summary>Номер участка</summary>
        public int NumberDepartment
        {
            get => _NumberDepartment;
            set
            {
                _NumberDepartment = value;
                OnPropertyChanged("NumberDepartment");
            }
        }
        #endregion

        #region Название участка
        private string _Name;

        /// <summary>Название участка</summary>
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        #endregion
    }
}
