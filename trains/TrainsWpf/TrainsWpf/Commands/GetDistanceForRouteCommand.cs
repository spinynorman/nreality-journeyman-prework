using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainsWpf.ViewModels;

namespace TrainsWpf.Commands
{
    public class GetDistanceForRouteCommand : ICommand
    {
        private TrainsMainViewModel _viewModel;

        public GetDistanceForRouteCommand(TrainsMainViewModel vm)
        {
            _viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.GetTrainGraph() != null;
        }

        public void Execute(object parameter)
        {
            _viewModel.GetTotalDistanceForRoute((string) parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
