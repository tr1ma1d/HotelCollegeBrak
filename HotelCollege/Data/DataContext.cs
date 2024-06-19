using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows;
using HotelCollege.Model;
using System.Runtime.Remoting.Contexts;
using System.Windows.Documents;

namespace HotelCollege.Data
{
    internal class DataContext
    {
        private string stringConnection = "Host=localhost;Port=5432; Database=hoteldb;Username=postgres;Password=admin";

        public async Task<NpgsqlConnection> ConnectToDatabaseAsync()
        {
            NpgsqlConnection conn = new NpgsqlConnection(stringConnection);
            await conn.OpenAsync();
            return conn;
        }

        public async Task CloseDatabaseAsync(NpgsqlConnection conn)
        {
            await conn.CloseAsync();
        }

        public async Task AddUsersAsync(string table, string context)
        {
            if (table != "" || table != null)
            {
                string query = $"INSERT INTO {table}(fullname, login, password, role) VALUES" + context;
                var conn = await ConnectToDatabaseAsync();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                await CloseDatabaseAsync(conn);
            }

        }

        public async Task RemoveAsync(string table, string context)
        {
            string query = $"DELETE FROM {table} WHERE" + context;
            var conn = await ConnectToDatabaseAsync();
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            await CloseDatabaseAsync(conn);
        }

        public async Task UpdateAsync(string table, string context)
        {
            string query = $"UPDATE {table} SET " + context;
            var conn = await ConnectToDatabaseAsync();
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            await CloseDatabaseAsync(conn);
        }
        public async Task<bool> CheckUser(string some, string username)
        {
            var c = await ConnectToDatabaseAsync();
            string query = $"SELECT {some} FROM employees WHERE login = @username";
            NpgsqlCommand command = new NpgsqlCommand(query, c);
            command.Parameters.AddWithValue("@username", username);

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                await CloseDatabaseAsync(c);
                return true;
            }
            await CloseDatabaseAsync(c);
            return false;
        }
        public async Task<string> SelectObjectEmployeeAsync(string username, string some)
        {
            string value = "";
            string user = User.Username;
            var c = await ConnectToDatabaseAsync();
            string query = $"SELECT {some} FROM employees WHERE login = @username";
            NpgsqlCommand command = new NpgsqlCommand(query, c);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@currentUser", user);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                value = reader[some].ToString();
            }
            await reader.CloseAsync();
            await CloseDatabaseAsync(c);
            return value;
        }

        public async Task<bool> ValidationFromDbAsync(string username, string password)
        {
            var c = await ConnectToDatabaseAsync();
            string query = $"SELECT password FROM employees WHERE login = @username";
            NpgsqlCommand command = new NpgsqlCommand(query, c);
            command.Parameters.AddWithValue("@username", username);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (password.Equals(reader["password"]))
                {
                    await CloseDatabaseAsync(c);
                    return true;
                }
            }
            await CloseDatabaseAsync(c);
            return false;
        }

        public async Task<NpgsqlDataReader> ShowTableAsync(string table)
        {
            var conn = await ConnectToDatabaseAsync();
            string query = $"SELECT * FROM {table}";
            NpgsqlCommand comm = new NpgsqlCommand(query, conn);
            var reader = await comm.ExecuteReaderAsync();

            return reader;
        }
        public async Task<NpgsqlDataReader> ShowRightJoinAsync(string table, string joinTable)
        {
            var conn = await ConnectToDatabaseAsync();
            string query = $"SELECT * FROM {table} INNER JOIN {joinTable} ON {table}.fk_id{joinTable} = {joinTable}.id";
            NpgsqlCommand comm = new NpgsqlCommand(query, conn);
            var reader = await comm.ExecuteReaderAsync();
            return reader;
        }
        public async Task<NpgsqlDataReader> ShowRightJoinAsync(string table, string joinTable, string secondJoinTable)
        {
            var conn = await ConnectToDatabaseAsync();
            string query = $"SELECT * FROM {table} RIGHT JOIN {joinTable} ON {table}.fk_id{joinTable} = {joinTable}.id RIGHT JOIN {secondJoinTable} ON {table}.fk_id{secondJoinTable} ";
            NpgsqlCommand comm = new NpgsqlCommand(query, conn);
            var reader = await comm.ExecuteReaderAsync();
            return reader;
        }
        public async Task<NpgsqlDataReader> ShowTableMany(string table, string joinTable, string secondJoinTable)
        {
            var conn = await ConnectToDatabaseAsync();
            string query = $"SELECT * FROM {table} RIGHT JOIN {joinTable} ON {table}.id{joinTable} = {joinTable}.id RIGHT JOIN {secondJoinTable} ON {table}.id{secondJoinTable} = {secondJoinTable}.id";
            NpgsqlCommand comm = new NpgsqlCommand(query, conn);
            var reader = await comm.ExecuteReaderAsync();
            return reader;
        }
        public async Task<NpgsqlDataReader> OneShowTableMany(string table, string joinTable, string secondJoinTable)
        {
            var conn = await ConnectToDatabaseAsync();
            string query = $"SELECT * FROM {table} RIGHT JOIN {joinTable} ON {table}.idroom = {joinTable}.id RIGHT JOIN {secondJoinTable} ON {table}.idservice = {secondJoinTable}.id";
            NpgsqlCommand comm = new NpgsqlCommand(query, conn);
            var reader = await comm.ExecuteReaderAsync();
            return reader;
        }
       
    }
}
