using HotelCollege.Model;
using HotelCollege.View;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HotelCollege.ViewModel
{
    internal class EmployeeViewModel : BindableBase
    {
        private string _name;
        private string _login;
        private string _password;
        private string _role;
        private string _status;

        private Employee _employee;

        private Employee _selectedEmployee;


        private ObservableCollection<Employee> _employees;
        internal Action<object, object> RequestClose;

        internal Action EmployeedAdded;
        internal Action EmployeedUpdated;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
                // Обновление привязок TextBox здесь
                if (_selectedEmployee != null)
                {
                    Name = _selectedEmployee.FullName;
                    Login = _selectedEmployee.Username;
                    Password = _selectedEmployee.Password;
                    Role = _selectedEmployee.Role.ToString(); 
                   // Предполагается, что Role - это перечисление
                }
            }
        }
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public string Role
        {
            get { return _role; }
            set { SetProperty(ref _role, value); }
        }
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public DelegateCommand Add { get; set; }
        public DelegateCommand Fired { get; set; }
        private ManagerViewModel _managerViewModel;
        public EmployeeViewModel()
        {
            
        
            Add = new DelegateCommand(async () => await AddingEmployee());
            Fired = new DelegateCommand(Fire);
            _employee = new Employee();
            InitializeAsync();
        }
        public EmployeeViewModel(ManagerViewModel managerViewModel)
        {
            _managerViewModel = managerViewModel;
            Add = new DelegateCommand(async () => await AddingEmployee());
            Fired = new DelegateCommand(Fire);
            _employee = new Employee();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            List<Employee> employeesList = await _employee.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(employeesList);
        }
        private async Task AddingEmployee()
        {

            _employee = new Employee();
          
            var result = await _employee.Add(Name, Login, Password, Role);
            if (result)
            {
              
                await RefreshEmployeesAsync();
                EmployeedAdded?.Invoke();
        

                _managerViewModel.Init();

                Application.Current.MainWindow.Close();
            }

        }
        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        public async Task RefreshEmployeesAsync()
        {
            List<Employee> employeesList = await _employee.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(employeesList);
        }
        private async void Fire()
        {
            await _employee.Remove(Login);
            EmployeedUpdated?.Invoke();
            _managerViewModel.Init();
        }
    }
}
