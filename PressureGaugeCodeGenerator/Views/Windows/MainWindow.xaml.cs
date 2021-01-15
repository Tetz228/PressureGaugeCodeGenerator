using PressureGaugeCodeGenerator.ViewModels;
using System.IO;
using System.Windows;

namespace PressureGaugeCodeGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.Closing += viewModel.MainWindow_Closing;
            this.Loaded += viewModel.MainWindow_Loaded;
        }
    }
}
