using Iktraktar.Models;
using System;
using System.Collections.Generic;

namespace Iktraktar
{
    internal static class OrderProcess
    {
        public static void ProcessOrder(Order order, Storage storage)
        {
            bool canProcess = true;
            var insufficientProducts = new List<OrderItem>();

            foreach (var item in order.Items)
            {
                var storedProduct = storage.FindById(item.Product.Id);
                if (storedProduct == null || storedProduct.Quantity < item.Quantity)
                {
                    canProcess = false;
                    insufficientProducts.Add(item);
                }
            }

            if (!canProcess)
            {
                Console.WriteLine($"Hiba: nem elég készlet a következő termékekből:");
                foreach (var item in insufficientProducts)
                {
                    Console.WriteLine($"#{item.Product.Id} {item.Product.Name} (kért: {item.Quantity}, raktáron: {storage.GetQuantity(item.Product)})");
                }
                return;
            }

            var deductedList = new List<string>();
            foreach (var item in order.Items)
            {
                storage.ReduceQuantity(item.Product, item.Quantity);
                deductedList.Add($"#{item.Product.Id} {item.Product.Name} (-{item.Quantity})");
            }

            Console.WriteLine($"\nRendelés feldolgozva #{order.Id}");
            Console.WriteLine("    Levont készlet: " + string.Join(", ", deductedList));
        }
    }
}