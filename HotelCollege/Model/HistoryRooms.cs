using HotelCollege.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelCollege.Model
{
    internal class HistoryRooms
    {
        DataContext _context;
        public int IdRoom { get; set; }
        public int IdServices { get; set; }
        public Room Rooms { get; set; }
        public Services Service { get; set; }

        public HistoryRooms()
        {
            _context = new DataContext();
        }

        public async Task<List<HistoryRooms>> GetHistoryAsync()
        {
            List<HistoryRooms> historyRooms = new List<HistoryRooms>();
            var conn = await _context.ConnectToDatabaseAsync();

            try
            {
                var reader = await _context.OneShowTableMany("rooms_services", "rooms", "services");
                try
                {
                    while (await reader.ReadAsync())
                    {
                        var idRoom = reader.IsDBNull(reader.GetOrdinal("idroom")) ? 0 : reader.GetInt32(reader.GetOrdinal("idroom"));
                        var idService = reader.IsDBNull(reader.GetOrdinal("idservice")) ? 0 : reader.GetInt32(reader.GetOrdinal("idservice"));
                        var roomId = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id"));
                        var roomName = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name"));
                        var serviceName = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name"));
                        var serviceDescription = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description"));

                        HistoryRooms rooms = new HistoryRooms
                        {
                            IdRoom = idRoom,
                            IdServices = idService,
                            Rooms = new Room
                            {
                                Id = roomId,
                                Name = roomName
                            },
                            Service = new Services
                            {
                                Id = idService,
                                Name = serviceName,
                                Description = serviceDescription
                            }
                        };
                        historyRooms.Add(rooms);
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
            return historyRooms;
        }
    }
}
