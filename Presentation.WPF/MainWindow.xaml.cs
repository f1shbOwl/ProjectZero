using Presentation.WPF.ViewModels;
using System.Windows;


namespace Presentation.WPF
{
    public partial class MainWindow : Window
    {
        
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

        }
    }
}