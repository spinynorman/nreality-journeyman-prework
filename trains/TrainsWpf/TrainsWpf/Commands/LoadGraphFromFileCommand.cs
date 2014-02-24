using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainsWpf.ViewModels;

namespace TrainsWpf.Commands
{
    public class LoadGraphFromFileCommand : ICommand
    {
        private TrainsMainViewModel _viewModel;

        public LoadGraphFromFileCommand(TrainsMainViewModel vm)
        {
            _viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter)
        {
            _viewModel.LoadGraphFromFile();
        }
    }
}
