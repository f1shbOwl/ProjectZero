using System.Windows;
using Presentation.WPF.ViewModels;



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