using StaffManager.Core.Domain;
using StaffManager.UI.Composition;
using StaffManager.UI.ViewModels;
using System.Windows;

namespace StaffManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var (csv, repo) = ServiceFactory.Create(StoreMode.Hash);
            var vm = new MainViewModel(csv, repo, StoreMode.Hash);
            new MainWindow { DataContext = vm }.Show();
        }
    }
}