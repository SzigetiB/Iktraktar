using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iktraktar.Models
{
    internal class OrderItem
    {
        public Product Product { get;}
        public int Quantity { get;}

        public OrderItem(Product product, int qty)
        {
            Product = product;
            Quantity = qty;
        }
    }
}
