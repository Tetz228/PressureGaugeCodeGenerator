namespace PressureGaugeCodeGenerator.Windows
{
    using System;
    using System.IO;
    using System.Windows;

    public partial class ListNumbersWindow : Window
    {
        private readonly string Patch;

        public ListNumbersWindow(string _patch)
        {
            Patch = _patch;
            InitializeComponent();
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListNumbersWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var streamReader = new StreamReader(Patch))
                {
                    string numbers = streamReader.ReadToEnd();
                    TextBoxNumbers.Text = numbers == "" ? "Номера в файле отсутствуют!" : numbers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при чтении файла с номерами", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
