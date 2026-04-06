using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views;

namespace AutoDrive.ViewModels
{
    public class FleetViewModel : ViewModelBase
    {
        // ----- Vehicles -----
        private readonly IMockVehicleService _vehicleService;
        private readonly IMockInstructorService _instructorService;
        private ObservableCollection<Vehicle> _vehicles = new();
        private Vehicle? _selectedVehicle;
        private string _vehicleSearch = string.Empty;

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public string VehicleSearch
        {
            get => _vehicleSearch;
            set
            {
                SetProperty(ref _vehicleSearch, value);
                _ = LoadVehicles();
            }
        }

        public RelayCommand LoadVehiclesCommand { get; }
        public RelayCommand AddVehicleCommand { get; }
        public RelayCommand EditVehicleCommand { get; }
        public RelayCommand DeleteVehicleCommand { get; }

        // ----- Instructors -----
        private ObservableCollection<Instructor> _instructors = new();
        private Instructor? _selectedInstructor;
        private string _instructorSearch = string.Empty;

        public ObservableCollection<Instructor> Instructors
        {
            get => _instructors;
            set => SetProperty(ref _instructors, value);
        }

        public Instructor? SelectedInstructor
        {
            get => _selectedInstructor;
            set => SetProperty(ref _selectedInstructor, value);
        }

        public string InstructorSearch
        {
            get => _instructorSearch;
            set
            {
                SetProperty(ref _instructorSearch, value);
                _ = LoadInstructors();
            }
        }

        public RelayCommand LoadInstructorsCommand { get; }
        public RelayCommand AddInstructorCommand { get; }
        public RelayCommand EditInstructorCommand { get; }
        public RelayCommand DeleteInstructorCommand { get; }

        public FleetViewModel(IMockVehicleService vehicleService, IMockInstructorService instructorService)
        {
            _vehicleService = vehicleService;
            _instructorService = instructorService;

            // Vehicles commands
            LoadVehiclesCommand = new RelayCommand(async _ => await LoadVehicles());
            AddVehicleCommand = new RelayCommand(async _ => await AddVehicle());
            EditVehicleCommand = new RelayCommand(async _ => await EditVehicle(), _ => SelectedVehicle != null);
            DeleteVehicleCommand = new RelayCommand(async _ => await DeleteVehicle(), _ => SelectedVehicle != null);

            // Instructors commands
            LoadInstructorsCommand = new RelayCommand(async _ => await LoadInstructors());
            AddInstructorCommand = new RelayCommand(async _ => await AddInstructor());
            EditInstructorCommand = new RelayCommand(async _ => await EditInstructor(), _ => SelectedInstructor != null);
            DeleteInstructorCommand = new RelayCommand(async _ => await DeleteInstructor(), _ => SelectedInstructor != null);

            _ = LoadVehicles();
            _ = LoadInstructors();
        }

        // ----- Vehicles methods -----
        private async Task LoadVehicles()
        {
            var all = await _vehicleService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(VehicleSearch))
            {
                var filtered = all.Where(v => v.Brand.Contains(VehicleSearch, System.StringComparison.OrdinalIgnoreCase) ||
                                              v.Model.Contains(VehicleSearch, System.StringComparison.OrdinalIgnoreCase) ||
                                              v.LicensePlate.Contains(VehicleSearch, System.StringComparison.OrdinalIgnoreCase)).ToList();
                Vehicles = new ObservableCollection<Vehicle>(filtered);
            }
            else
            {
                Vehicles = new ObservableCollection<Vehicle>(all);
            }
        }

        private async Task AddVehicle()
        {
            var newVehicle = new Vehicle();
            var instructors = await _instructorService.GetAllAsync();
            var dialog = new VehicleEditDialog(newVehicle, instructors);
            if (dialog.ShowDialog() == true)
            {
                await _vehicleService.CreateAsync(newVehicle);
                await LoadVehicles();
            }
        }

        private async Task EditVehicle()
        {
            if (SelectedVehicle == null) return;
            var copy = new Vehicle
            {
                Id = SelectedVehicle.Id,
                Brand = SelectedVehicle.Brand,
                Model = SelectedVehicle.Model,
                LicensePlate = SelectedVehicle.LicensePlate,
                Year = SelectedVehicle.Year,
                InsuranceExpiry = SelectedVehicle.InsuranceExpiry,
                InspectionExpiry = SelectedVehicle.InspectionExpiry,
                MaintenanceDue = SelectedVehicle.MaintenanceDue,
                Status = SelectedVehicle.Status,
                InstructorId = SelectedVehicle.InstructorId
            };
            var instructors = await _instructorService.GetAllAsync();
            var dialog = new VehicleEditDialog(copy, instructors);
            if (dialog.ShowDialog() == true)
            {
                await _vehicleService.UpdateAsync(copy.Id, copy);
                await LoadVehicles();
            }
        }

        private async Task DeleteVehicle()
        {
            if (SelectedVehicle == null) return;
            if (MessageBox.Show($"Удалить автомобиль {SelectedVehicle.Brand} {SelectedVehicle.Model} ({SelectedVehicle.LicensePlate})?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _vehicleService.DeleteAsync(SelectedVehicle.Id);
                await LoadVehicles();
            }
        }

        // ----- Instructors methods -----
        private async Task LoadInstructors()
        {
            var all = await _instructorService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(InstructorSearch))
            {
                var filtered = all.Where(i => i.LastName.Contains(InstructorSearch, System.StringComparison.OrdinalIgnoreCase) ||
                                              i.FirstName.Contains(InstructorSearch, System.StringComparison.OrdinalIgnoreCase) ||
                                              i.Phone.Contains(InstructorSearch)).ToList();
                Instructors = new ObservableCollection<Instructor>(filtered);
            }
            else
            {
                Instructors = new ObservableCollection<Instructor>(all);
            }
        }

        private async Task AddInstructor()
        {
            var newInstructor = new Instructor();
            var dialog = new InstructorEditDialog(newInstructor);
            if (dialog.ShowDialog() == true)
            {
                await _instructorService.CreateAsync(newInstructor);
                await LoadInstructors();
            }
        }

        private async Task EditInstructor()
        {
            if (SelectedInstructor == null) return;
            var copy = new Instructor
            {
                Id = SelectedInstructor.Id,
                LastName = SelectedInstructor.LastName,
                FirstName = SelectedInstructor.FirstName,
                MiddleName = SelectedInstructor.MiddleName,
                Phone = SelectedInstructor.Phone,
                Email = SelectedInstructor.Email,
                HourlyRate = SelectedInstructor.HourlyRate,
                IsActive = SelectedInstructor.IsActive
            };
            var dialog = new InstructorEditDialog(copy);
            if (dialog.ShowDialog() == true)
            {
                await _instructorService.UpdateAsync(copy.Id, copy);
                await LoadInstructors();
            }
        }

        private async Task DeleteInstructor()
        {
            if (SelectedInstructor == null) return;
            if (MessageBox.Show($"Удалить инструктора {SelectedInstructor.LastName} {SelectedInstructor.FirstName}?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _instructorService.DeleteAsync(SelectedInstructor.Id);
                await LoadInstructors();
            }
        }
    }
}