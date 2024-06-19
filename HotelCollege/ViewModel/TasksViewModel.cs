using HotelCollege.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollege.ViewModel
{
    internal class TasksViewModel : BindableBase
    {
        private Tasks _tasks;
        private Employee _employee;
        private string _searchName;


        private ObservableCollection<Employee> employees;  
        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { SetProperty(ref employees, value); }
        }
        public string SearchName
        {
            get { return _searchName; }
            set { SetProperty(ref _searchName, value); }
        }
        
        public Delegate ShowEmp { get; set; }
        public TasksViewModel DataContext { get; internal set; }

        public TasksViewModel()
        {
            InitializeAsync();
        }
        public TasksViewModel(ManagerViewModel managerViewModel)
        {
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            List<Employee> employeesList = await _employee.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(employeesList);
        }
    }
}
