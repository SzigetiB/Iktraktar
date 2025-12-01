using Iktraktar.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Iktraktar
{
    internal class Program
    {
        static int nextOrderId = 1;
        static List<Order> orders = new List<Order>();

        static void Main(string[] args)
        {
            Storage storage = new Storage();
            storage.Add(new Product(1, "Ceruza", 100));
            storage.Add(new Product(2, "Toll", 50));
            storage.Add(new Product(3, "Füzet", 80));

            bool running = true;
            string filePath = "raktar.csv";

            while (running)
            {
                Console.WriteLine("\n--- Raktárkezelő ---");
                Console.WriteLine("1. Termékek listázása");
                Console.WriteLine("6. Rendelés létrehozása termékekből (csak elérhető)");
                Console.WriteLine("7. Rendelés összegzése, kiírás fájlba");
                Console.WriteLine("8. Rendelés feldolgozása");
                Console.WriteLine("9 - Raktár betöltése CSV-ből");
                Console.WriteLine("10 - Raktár mentése CSV-be");
                Console.WriteLine("0. Kilépés");
                Console.Write("Válassz menüpontot: ");

                string choice = Console.ReadLine()!;

                switch (choice)
                {
                    case "1":
                        storage.PrintAllProducts();
                        break;

                    case "6":
                        CreateOrderFromAvailable(storage);
                        break;

                    case "7":
                        ExportOrdersToFile();
                        break;

                    case "8":
                        ProcessOrders(storage);
                        break;

                    case "9":
                        storage.Load(filePath);
                        Console.WriteLine($"\nRaktár betöltve: {filePath}");
                        break;

                    case "10":
                        storage.Save(filePath);
                        Console.WriteLine($"\nRaktár mentve: {filePath}");
                        break;


                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Hibás menüpont!");
                        break;
                }
            }
        }

        static void CreateOrderFromAvailable(Storage storage)
        {
            Order order = new Order(nextOrderId);
            Console.WriteLine($"\nRendelés létrehozva. Order ID: {nextOrderId}");
            nextOrderId++;

            bool addingItems = true;

            while (addingItems)
            {
                Console.WriteLine("\n--- Elérhető termékek ---");
                foreach (var p in storage.GetAllProducts())
                {
                    if (p.Quantity > 0)
                        Console.WriteLine($"{p.Id} - {p.Name} ({p.Quantity} db)");
                }

                Console.Write("Termék ID: ");
                int productId = int.Parse(Console.ReadLine()!);
                var product = storage.FindById(productId);

                if (product == null || product.Quantity == 0)
                {
                    Console.WriteLine("Nem elérhető termék!");
                    continue;
                }

                Console.Write("Mennyiség: ");
                int qty = int.Parse(Console.ReadLine()!);

                if (qty > product.Quantity)
                {
                    Console.WriteLine("Nincs ennyi készlet!");
                    continue;
                }

                order.AddItem(product, qty);

                Console.Write("További termék? (i/n): ");
                string cont = Console.ReadLine()!;
                if (cont.ToLower() != "i") addingItems = false;
            }

            orders.Add(order);
            Console.WriteLine("\nRendelés kész, de még nem feldolgozott!");
        }

        static void ExportOrdersToFile()
        {
            string fileName = "orders_summary.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var order in orders)
                {
                    writer.WriteLine($"Order ID: {order.Id}");
                    foreach (var item in order.Items)
                    {
                        writer.WriteLine($"- {item.Product.Name}: {item.Quantity} db");
                    }
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"Rendelések kiírva a fájlba: {fileName}");
        }

        static void ProcessOrders(Storage storage)
        {
            foreach (var order in orders)
            {
                OrderProcess.ProcessOrder(order, storage);
            }

            Console.WriteLine("\nÖsszes rendelés feldolgozva.");
        }
    }
}