using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StaffManager.Core.Abstractions;
using StaffManager.UI.Composition;

namespace StaffManager.UI.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly IStaffRepository _repo;

        private readonly ICsvSerialiser _csvSerialiser;
        public event EventHandler? RequestClose;

        public ObservableCollection<KeyValuePair<int, string>> StaffEntries { get; } = new();

        private int _staffId;
        public int StaffId
        {
            get => _staffId;
            set => SetProperty(ref _staffId, value);
        }

        private string _staffName = string.Empty;
        public string StaffName
        {
            get => _staffName;
            set => SetProperty(ref _staffName, value);
        }

        private bool _isStaffIdReadOnly;
        public bool IsStaffIdReadOnly
        {
            get => _isStaffIdReadOnly;
            set => SetProperty(ref _isStaffIdReadOnly, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }
        private string _statusMessage = "Ready";

        public ICommand AddStaffCommand { get; }
        public ICommand UpdateStaffCommand { get; }
        public ICommand RemoveStaffCommand { get; }
        public ICommand CloseAdminPanelKeybind { get; }

        //Loads admin panel with optional seed data
        public AdminViewModel(IStaffRepository repo, ICsvSerialiser csvSerialiser, AdminSeed? staff = null)
        {
            _csvSerialiser = csvSerialiser;
            _repo = repo;

            if (staff is null)
            {
                IsStaffIdReadOnly = false;
                StaffId = 77;
            }
            else
            {
                IsStaffIdReadOnly = staff.IsStaffIdReadOnly;
                if (staff.StaffId.HasValue) StaffId = staff.StaffId.Value;
                if (!string.IsNullOrWhiteSpace(staff.StaffName)) StaffName = staff.StaffName!;
                if (!staff.StaffId.HasValue && string.IsNullOrWhiteSpace(staff.StaffName))
                {
                    IsStaffIdReadOnly = false;
                    StaffId = 77;
                }
            }

            AddStaffCommand = new RelayCommand(_ => AddStaff());
            UpdateStaffCommand = new RelayCommand(_ => UpdateStaff());
            RemoveStaffCommand = new RelayCommand(_ => RemoveStaff());
            CloseAdminPanelKeybind = new RelayCommand(_ => ClosePanel());
        }
        //Adds new staff entry
        private void AddStaff()
        {
            if (string.IsNullOrWhiteSpace(StaffName)) 
            { 
                StatusMessage = "Name is required."; return; 
            }
            try 
            { 
                _repo.Add(StaffId, StaffName); 
                StatusMessage = $"Added {StaffId} — {StaffName}"; 
                ClearPanel(); 
            }
            catch (Exception ex) 
            { 
                StatusMessage = ex.Message; 
            }
        }
        //Updates existing staff entry
        private void UpdateStaff()
        {
            try 
            {
                _repo.UpdateName(StaffId, StaffName); 
                StatusMessage = $"Updated {StaffId} — {StaffName}"; 
                ClearPanel(); 
            }
            catch (Exception ex) { StatusMessage = ex.Message; }
        }
        //Removes staff entry
        private void RemoveStaff()
        {
            if (_repo.Remove(StaffId)) { StatusMessage = $"Removed {StaffId}"; ClearPanel(); }
            else { StatusMessage = "ID not found."; }
        }
        //Clears input fields in the admin panel
        private void ClearPanel()
        {
            StaffId = 77;
            StaffName = string.Empty;
            IsStaffIdReadOnly = false;
        }
        //Closes the admin panel and saves data to CSV
        private void ClosePanel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
            _csvSerialiser.Save("staff_master.csv", _repo.All());
        }
    }
}
