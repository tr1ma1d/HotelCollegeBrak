using HotelCollege.Model;
using HotelCollege.View;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Threading.Tasks; // Использование пространства имен для асинхронных операций
using System.Windows;

namespace HotelCollege.ViewModel
{
    internal class AuthViewModel : BindableBase
    {
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand LogInCommand { get; private set; }

        public AuthViewModel()
        {
            LogInCommand = new DelegateCommand(async () => await AuthEmployee());
        }

        private async Task AuthEmployee()
        {
            Employee employee = new Employee();
            int numberWindow = await employee.CheckEmployeeAsync(_username, _password);
            MoveWindow(numberWindow);
        }

        private void MoveWindow(int number)
        {
            if (number == 1)
            {
                ManagerWindow managerWindow = new ManagerWindow();
                managerWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else if (number == 2)
            {
                ProviderWindow providerWindow = new ProviderWindow();
                providerWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else if (number == 3)
            {
                ServiceRoomWindow serviceRoomWindow = new ServiceRoomWindow();
                serviceRoomWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
