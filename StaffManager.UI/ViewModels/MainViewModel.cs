using System.Collections.ObjectModel;
using System.Windows.Input;
using StaffManager.Core.Abstractions;
using StaffManager.Core.Domain;
using StaffManager.UI.Composition;
using System.Windows;

namespace StaffManager.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICsvSerialiser _csv;
        private IStaffRepository _repo;


        private StoreMode _currentStoreMode;
        public StoreMode CurrentStoreMode
        {
            get => _currentStoreMode;
            set
            {
                if (SetProperty(ref _currentStoreMode, value))
                    ChangeStoreMode(value);
            }
        }
        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private string _filterName = "";
        public string FilterName
        {
            get => _filterName;
            set
            {
                if (SetProperty(ref _filterName, value))
                    ApplyFilter();
            }
        }

        private string _filterId = "";
        public string FilterId
        {
            get => _filterId;
            set
            {
                if (SetProperty(ref _filterId, value))
                    ApplyFilter();
            }
        }

        public ObservableCollection<string> UnsortedEntries { get; } = new();
        public ObservableCollection<string> FilteredEntries { get; } = new();

        public ICommand LoadCsvCommand { get; }
        public ICommand OpenAdminCommand { get; }

        public MainViewModel(ICsvSerialiser csv, IStaffRepository repo, StoreMode initialMode)
        {
            _csv = csv;
            _repo = repo;
            _currentStoreMode = initialMode;

            LoadCsvCommand = new RelayCommand(_ => LoadCsv());
            OpenAdminCommand = new RelayCommand(_ => OpenAdmin());
            RefreshLists();
        }

        private void OpenAdmin()
        {
            var nameEmpty = string.IsNullOrWhiteSpace(FilterName);
            var idEmpty = string.IsNullOrWhiteSpace(FilterId);

            AdminSeed seed;

            if (nameEmpty && idEmpty)
            {
                seed = new AdminSeed
                {
                    StaffId = 77,
                    StaffName = null,
                    IsStaffIdReadOnly = false
                };
            }
            else if (!nameEmpty && !idEmpty && int.TryParse(FilterId.Trim(), out var parsedId))
            {
                seed = new AdminSeed
                {
                    StaffId = parsedId,
                    StaffName = FilterName.Trim(),
                    IsStaffIdReadOnly = true
                };
            }
            else
            {
                int? maybeId = null;
                if (int.TryParse(FilterId?.Trim(), out var pid)) maybeId = pid;

                seed = new AdminSeed
                {
                    StaffId = maybeId,
                    StaffName = string.IsNullOrWhiteSpace(FilterName) ? null : FilterName.Trim(),
                    IsStaffIdReadOnly = false
                };
            }

            var adminVm = new AdminViewModel(_repo, _csv, seed);
            var win = new AdminWindow(adminVm) { Owner = Application.Current.MainWindow };
            win.ShowDialog();
            RefreshLists();
        }

        private void LoadCsv()
        {
            var path = "staff_master.csv";
            if (path is null)
            {
                StatusMessage = "Load cancelled.";
                return;
            }

            var records = _csv.Load(path);
            _repo.ReplaceAll(records);
            StatusMessage = $"Loaded records.";
            RefreshLists();
        }

        private void ChangeStoreMode(StoreMode newMode)
        {
            var snapshot = _repo.All().ToArray();
            _repo = ServiceFactory.CreateRepository(newMode, snapshot);
            RefreshLists();
            StatusMessage = $"Switched to {newMode}.";
        }

        private void RefreshLists()
        {
            UnsortedEntries.Clear();
            foreach (var kv in _repo.All())
                UnsortedEntries.Add($"{kv.Key} — {kv.Value}");

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredEntries.Clear();

            var name = _filterName.Trim().ToLowerInvariant();
            var id = _filterId.Trim();

            var query = _repo.AllFiltered();
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(id))
                return;

            if (!string.IsNullOrEmpty(name))
                query = query.Where(kv => kv.Value.ToLowerInvariant().Contains(name));

            if (!string.IsNullOrEmpty(id))
                query = query.Where(kv => kv.Key.ToString().StartsWith(id));

            foreach (var kv in query)
                FilteredEntries.Add($"{kv.Key} — {kv.Value}");
        }
    }
}