using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollege.Model
{
    internal class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{ID} Names: {Name}";
        }

    }
}
