using HotelCollege.Data;
using HotelCollege.Model;
using HotelCollege.View;
using Npgsql;
using Npgsql.Replication;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelCollege.ViewModel
{
    internal class ManagerViewModel : BindableBase
    {
        private Employee _employee;
        private Order _order;
        private HistoryRooms _historyRoom;

        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Order> _orders;
        private ObservableCollection<HistoryRooms> _historyRooms;

        private DataContext _context;



        public event Action DataUpdated;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }
        public ObservableCollection<HistoryRooms> Histories
        {
            get { return _historyRooms; }
            set { SetProperty(ref _historyRooms, value); }
        }

        public DelegateCommand OpenFirst{ get; private set; }
        public DelegateCommand OpenSecond { get; private set; }

        public DelegateCommand ReturnCommand { get; private set; }

        public EventHandler RequestClose;

    
        public ManagerViewModel()
        {

            OpenFirst = new DelegateCommand(OpenWindowFirst);
            OpenSecond = new DelegateCommand(OpenWindowSecond);

            ReturnCommand = new DelegateCommand(MoveBack);
            _employee = new Employee();
            _order = new Order();
            _historyRoom = new HistoryRooms();

            InitializeAsync();

            DataUpdated += () =>
            {
                // Обновляем данные сотрудников
                InitializeAsync();
            };
        }
        
        private async void InitializeAsync()
        {
            
        
            List<Employee> employeesList = await _employee.GetEmployeesAsync();
            List<Order> orderList = await _order.GetOrdersAsync();
            List<HistoryRooms> historyList = await _historyRoom.GetHistoryAsync();
            Orders = new ObservableCollection<Order>(orderList);
            Employees = new ObservableCollection<Employee>(employeesList);
            Histories = new ObservableCollection<HistoryRooms>(historyList);

        }
        public void Init()
        {
            InitializeAsync();
        }
        private void OpenWindowFirst()
        {

            EmployeeWindow employeeWindow = new EmployeeWindow();

            // Передаем текущий экземпляр ManagerViewModel в EmployeeViewModel
            var employeeViewModel = new EmployeeViewModel(this);
            employeeWindow.DataContext = employeeViewModel;

            employeeWindow.Show();

          
        }
        private void OpenWindowSecond()
        {

            TasksWindow tasksWindow = new TasksWindow();

            // Передаем текущий экземпляр ManagerViewModel в EmployeeViewModel
            var tasksViewModel = new TasksViewModel(this);
            tasksViewModel.DataContext = tasksViewModel;

            tasksWindow.Show();
        }
        private void MoveBack()
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            Application.Current.MainWindow.Close();
        }
        public async Task RefreshEmployeesAsync()
        {
            List<Employee> employeesList = await _employee.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(employeesList);
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
