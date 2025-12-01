using Iktraktar.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iktraktar.Models
{
    internal class Order : IIndentifiable
    {
        public int Id { get; }
        public List<OrderItem> Items { get; } = new List<OrderItem>();

        public Order(int id)
        {
            Id = id;
        }
        public void AddItem(Product product, int quantity)
        {
            Items.Add(new OrderItem(product, quantity));
        }

        public void SaveSummaryToFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Rendelés #{Id}");
                writer.WriteLine("------------------------");

                foreach (var item in Items)
                {
                    writer.WriteLine($"#{item.Product.Id} {item.Product.Name} x {item.Quantity}");
                }
            }
        }
        // Összegzés a konzolra (teszteléshez)
        public void PrintSummary()
        {
            Console.WriteLine($"Rendelés #{Id}");
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
        }
    }
}