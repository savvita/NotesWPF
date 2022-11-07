using System.Windows;
using System.Windows.Controls;
using NotesWPF.ViewModel;

namespace NotesWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }

        private void CancelButton_Click (object sender, RoutedEventArgs e)
        {
            TitleTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            ContentTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }
    }
}
