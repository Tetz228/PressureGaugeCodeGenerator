using PressureGaugeCodeGeneratorTestWpf.Commands;
using System.Collections.Generic;
using System.Windows;

namespace PressureGaugeCodeGeneratorTestWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>
            {
                { "Department", comboBox_department.Text },
                { "Width", textBox_width.Text },
                { "Height", textBox_height.Text },
                { "Width_BMP", textBox_width_bmp.Text },
                { "Height_BMP", textBox_height_bmp.Text },
                { "Checked", checkBox_setYear.IsChecked.ToString() },
                { "Format", comboBox_format.Text }
            };
            Settings.SaveSettings(settings);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> settings = Settings.ReadSettings();

            if (settings.Count != 0)
            {
                textBox_width.Text = settings["Width"];
                textBox_height.Text = settings["Height"];
                textBox_width_bmp.Text = settings["Width_BMP"];
                textBox_height_bmp.Text = settings["Height_BMP"];
                checkBox_setYear.IsChecked = bool.Parse(settings["Checked"]);
                switch (settings["Department"])
                {
                    case "1 - Литография (ППШ)":
                        comboBox_department.SelectedIndex = 0;
                        break;
                    case "2 - Безрегулировка":
                        comboBox_department.SelectedIndex = 1;
                        break;
                    case "3 - Безрегулировка (штучный циферблат)":
                        comboBox_department.SelectedIndex = 2;
                        break;
                    case "4 - ПНП":
                        comboBox_department.SelectedIndex = 3;
                        break;
                }
                switch (settings["Format"])
                {
                    case "BMP":
                        comboBox_format.SelectedIndex = 0;
                        break;
                    case "PNG":
                        comboBox_format.SelectedIndex = 1;
                        break;
                    case "JPEG":
                        comboBox_format.SelectedIndex = 2;
                        break;
                    case "BMP + PNG":
                        comboBox_format.SelectedIndex = 3;
                        break;
                }
            }
        }
    }
}
