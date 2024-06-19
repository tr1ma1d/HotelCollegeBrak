using HotelCollege.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelCollege.Model
{
    internal class Order
    {
        private DataContext _context;

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }

        public Order()
        {
            _context = new DataContext();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            List<Order> orders = new List<Order>();
            var conn = await _context.ConnectToDatabaseAsync();
            try
            {
                var reader = await _context.ShowRightJoinAsync("orders", "customers");
                try
                {
                    while (await reader.ReadAsync())
                    {
                        Order order = new Order
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            Date = reader["date"] != DBNull.Value ? Convert.ToDateTime(reader["date"]) : DateTime.MinValue,
                            CheckInDate = reader["check_in_date"] != DBNull.Value ? Convert.ToDateTime(reader["check_in_date"]) : DateTime.MinValue,
                            CheckOutDate = reader["check_out_date"] != DBNull.Value ? Convert.ToDateTime(reader["check_out_date"]) : DateTime.MinValue,
                            CustomerId = reader["fk_idcustomers"] != DBNull.Value ? Convert.ToInt32(reader["fk_idcustomers"]) : 0,
                          
                            Customers = new Customer()
                            {
                                ID = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                                Name = reader["fullname"] != DBNull.Value ? reader["fullname"].ToString() : string.Empty
                            }
                        };
                        orders.Add(order);
                    }
                }
                finally
                {
                    await reader.CloseAsync();
                }
            }
            finally
            {
                await _context.CloseDatabaseAsync(conn);
            }
            return orders;
        }
    }
}
