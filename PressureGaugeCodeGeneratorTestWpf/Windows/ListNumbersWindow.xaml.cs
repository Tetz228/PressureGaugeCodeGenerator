using System.Windows;

namespace PressureGaugeCodeGeneratorTestWpf.Windows
{
    public partial class ListNumbersWindow : Window
    {
        public ListNumbersWindow()
        {
            InitializeComponent();
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
