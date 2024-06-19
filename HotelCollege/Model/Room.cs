using HotelCollege.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelCollege.Model
{
    
    internal class Room
    {
        private DataContext _context;
        public int Id { get; set; }
        public string Name { get; set; }
        public Room()
        {
            _context = new DataContext();
        }
        public override string ToString()
        {
            return $"Room ID: {Id}, Name: {Name}";
        }
        public async Task<List<Room>> GetRoomsAsync()
        {
            List<Room> roomsList = new List<Room>();
            var conn = await _context.ConnectToDatabaseAsync();
            
            try
            {
                var reader = await _context.ShowTableMany("rooms_services", "rooms", "services");
                try
                {
                    while (await reader.ReadAsync())
                    {
                        Room room = new Room()
                        {
                            
                        };
                    }
                }
                finally
                {

                }
               
            }
            finally
            {
                
            }


            await _context.CloseDatabaseAsync(conn);
            return roomsList;
        }
    }
}
