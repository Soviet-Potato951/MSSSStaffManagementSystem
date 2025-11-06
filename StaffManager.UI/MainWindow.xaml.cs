using StaffManager.UI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StaffManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.LoadCsvCommand.CanExecute(null))
                vm.LoadCsvCommand.Execute(null);
        }

        private void FocusName_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NameFilterTextBox.Clear();
            NameFilterTextBox.Focus();
        }

        private void FocusId_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IdFilterTextBox.Clear();
            IdFilterTextBox.Focus();
        }

        private void PopulateFromSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (lstFiltered.SelectedItem is string record)
            {
                // expected format: "123 — John Doe"
                var parts = record.Split('—');
                if (parts.Length == 2)
                {
                    IdFilterTextBox.Text = parts[0].Trim();
                    NameFilterTextBox.Text = parts[1].Trim();
                }
            }
        }
    }
}