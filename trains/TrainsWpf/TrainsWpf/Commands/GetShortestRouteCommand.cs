using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrainsWpf.ViewModels;

namespace TrainsWpf.Commands
{
    public class GetShortestRouteCommand : ICommand
    {
        private TrainsMainViewModel _viewModel;

        public GetShortestRouteCommand(TrainsMainViewModel vm)
        {
            _viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.GetTrainGraph() != null;
        }

        public void Execute(object parameter)
        {
            _viewModel.FindShortestRoute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
