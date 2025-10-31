using System.Windows;
using StaffManager.UI.ViewModels;

namespace StaffManager.UI
{
    public partial class AdminWindow : Window
    {
        public AdminWindow(AdminViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.RequestClose += (_, __) => this.Close();
        }
    }
}
