using PressureGaugeCodeGenerator.Infrastructure.Commands.Base;
using PressureGaugeCodeGenerator.Views.Windows;

namespace PressureGaugeCodeGenerator.Infrastructure.Commands
{
    class OpenAboutProgramCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            AboutProgram aboutProgram = new AboutProgram();
            aboutProgram.ShowDialog();
        }
    }
}
