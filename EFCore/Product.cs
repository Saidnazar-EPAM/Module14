using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
