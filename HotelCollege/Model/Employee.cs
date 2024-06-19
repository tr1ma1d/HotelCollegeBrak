using HotelCollege.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace HotelCollege.Model
{
    public enum Roles
    {
        None = 0,
        Manager = 1,
        ServiceRoom = 2,
        Provider = 3
    }

    internal class Employee
    {
        public int ID { get; set; }
        
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public Roles Role { get; set; }
        DataContext context;

        public Employee()
        {
            context = new DataContext();
        }

        public async Task<int> CheckEmployeeAsync(string username, string password)
        {
            if (username == null || password == null)
            {
                return 0;
            }

            if (await context.ValidationFromDbAsync(username, password))
            {
                User.Username = username;
                var roleString = await context.SelectObjectEmployeeAsync(username, "role");
                Roles role = MapRole(roleString);
                if (role != Roles.None)
                {
                    return (int)role;
                }
                else
                {
                    return 0; // Неизвестная роль
                }
            }
            return 0;
        }

        protected Roles MapRole(string roleString)
        {
            switch (roleString)
            {
                case "Менеджер":
                    return Roles.Manager;
                case "Сотрудник обслуживания номеров":
                    return Roles.ServiceRoom;
                case "Сотрудник предоставление услуг":
                    return Roles.Provider;
                default:
                    return Roles.None; // Перечисление "None" для неизвестной роли
            }
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            List<Employee> employees = new List<Employee>();
            var conn = await context.ConnectToDatabaseAsync();
            var reader = await context.ShowTableAsync("employees");

            while (await reader.ReadAsync())
            {
                Username = reader["login"].ToString();
                Employee employee = new Employee
                {
                    ID = Convert.ToInt32(reader["id"]),
                    FullName = reader["fullname"].ToString(),
                    Username = reader["login"].ToString(),
                    Password = reader["password"].ToString(), // Предполагая, что есть поле password
                    Role = MapRole(reader["role"].ToString()),
                    Status = reader["status"].ToString(),
                };

                employees.Add(employee);
            }

            await reader.CloseAsync();
            await context.CloseDatabaseAsync(conn);
            return employees;
        }
        public async Task<bool> Add(string name, string login ,string password, string role)
        {
            string query = $"('{name}', '{login}', '{password}', '{role}')";
            if (!await context.CheckUser("employees", login))
            {
                await context.AddUsersAsync("employees", query);
                MessageBox.Show("Успешно");
                return true;
            }
            else
            {
                MessageBox.Show("Пользователь существует");
                return false;
            }
     
           

        }
        public async Task Remove(string username)
        {
            string query = $"status = 'Уволен' WHERE login = '{username}'";
            await context.UpdateAsync("employees", query);
            MessageBox.Show("Сотрудник уволен");
        }
    }
}
