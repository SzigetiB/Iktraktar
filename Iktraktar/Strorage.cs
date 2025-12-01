using Iktraktar.Models.Interfaces;
using System;
using System.Collections.Generic;

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
    }
}