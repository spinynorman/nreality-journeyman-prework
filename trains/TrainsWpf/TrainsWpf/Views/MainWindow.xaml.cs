using System.Windows;
using TrainsWpf.ViewModels;

namespace TrainsWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new TrainsMainViewModel(GgArea);
            DataContext = vm;
        }
    }
}
