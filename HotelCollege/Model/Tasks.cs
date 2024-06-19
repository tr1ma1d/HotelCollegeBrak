using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollege.Model
{
    internal class Tasks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdEmployee { get; set; }
        public int IdServices { get; set; }
        public int IdRoom { get; set; }
        public Employee Employees { get; set; }
        public Services Service { get; set; }
        public Room Rooms { get; set; }
    }
}
