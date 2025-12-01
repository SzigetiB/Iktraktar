using Iktraktar.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iktraktar
{
    internal class Product : IIndentifiable, IStorable
    {
        public int Id { get;}

        public string Name {get; }
        public int Quantity { get;set;}

        public Product(int id, string name,int qty)
        {
            Id = id;
            Name = name;
            Quantity = qty;
        }
        public override string ToString()
        {
            return $"#{Id} | {Name} | Készlet: {Quantity}";
        }
    }
}
