using Iktraktar.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Iktraktar.Models
{
    internal class Storage : ISearchable<Product>
    {
        private List<Product> items = new List<Product>();

        public void Add(Product product)
        {
            items.Add(product);
        }

        public Product? FindById(int id)
        {
            foreach (var item in items)
            {
                if (item.Id == id) return item;
            }
            return null;
        }

        public IEnumerable<Product> FindAll(string name)
        {
            List<Product> searchedProducts = new List<Product>();
            foreach (Product product in items)
            {
                if (product.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    searchedProducts.Add(product);
                }
            }
            return searchedProducts;
        }

        public int GetQuantity(Product product)
        {
            var p = FindById(product.Id);
            return p != null ? p.Quantity : 0;
        }

        public bool ReduceQuantity(Product product, int amount)
        {
            var p = FindById(product.Id);
            if (p == null || p.Quantity < amount)
                return false;

            p.Quantity -= amount;
            return true;
        }

        public void PrintAllProducts()
        {
            Console.WriteLine("Raktár tartalma:");
            foreach (var p in items)
            {
                Console.WriteLine($"{p.Id} - {p.Name} ({p.Quantity} db)");
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return items;
        }
        public void Save(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Id;Name;Quantity");
                foreach (var p in items)
                {
                    writer.WriteLine($"{p.Id};{p.Name};{p.Quantity}");
                }
            }
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("A fájl nem található!");
                return;
            }

            var lines = File.ReadAllLines(path);
            items.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(';');
                Console.WriteLine($"Betöltött sor {i}: {lines[i]}, részek száma: {parts.Length}");
                if (parts.Length != 3)
                {
                    Console.WriteLine("A sor formátuma hibás, kihagyva.");
                    continue;
                }

                if (!int.TryParse(parts[0], out int id))
                {
                    Console.WriteLine("Az ID nem szám, kihagyva.");
                    continue;
                }
                if (!int.TryParse(parts[2], out int qty))
                {
                    Console.WriteLine("A Quantity nem szám, kihagyva.");
                    continue;
                }

                string name = parts[1];
                items.Add(new Product(id, name, qty));

            }
            Console.WriteLine($"Összes termék betöltve: {items.Count}");
        }


    }
}