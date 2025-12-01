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

            while (running)
            {
                Console.WriteLine("\n--- Raktárkezelő ---");
                Console.WriteLine("1 - Keresés ID alapján");
                Console.WriteLine("2 - Keresés név részlet alapján");
                Console.WriteLine("3 - Teljes lista formázott megjelenítése");
                Console.WriteLine("4 - Készlet növelése");
                Console.WriteLine("5 - Készlet csökkentése");
                Console.WriteLine("6. Rendelés létrehozása termékekből (csak elérhető)");
                Console.WriteLine("7. Rendelés összegzése, kiírás fájlba");
                Console.WriteLine("8. Rendelés feldolgozása");
                Console.WriteLine("0. Kilépés");
                Console.Write("Válassz menüpontot: ");

                string choice = Console.ReadLine()!;

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Termék ID: ");
                        int searchId = int.Parse(Console.ReadLine());

                        var foundItem = storage.FindById(searchId);
                        if (foundItem != null)
                        {
                            Console.WriteLine("ID: " + foundItem.Id);
                            Console.WriteLine("Név: " + foundItem.Name);
                            Console.WriteLine("Mennyiség: " + foundItem.Quantity);
                        }
                        else
                        {
                            Console.WriteLine("Nincs termék ilyen ID-vel!");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Név: ");
                        break;

                    case "4":
                        Console.Write("Termék ID: ");
                        int incId = int.Parse(Console.ReadLine());

                        Console.Write("Mennyivel növeljem?: ");
                        int incAmount = int.Parse(Console.ReadLine());

                        IncreaseQuantity(storage, incId, incAmount);
                        break;

                    case "5":
                        Console.Write("Termék ID: ");
                        int decId = int.Parse(Console.ReadLine());

                        Console.Write("Mennyivel csökkentsem?: ");
                        int decAmount = int.Parse(Console.ReadLine());

                        DecreaseQuantity(storage, decId, decAmount);
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
        static void IncreaseQuantity(Storage storage, int id, int amount)
        {
            var product = storage.FindById(id);

            if (product == null)
            {
                Console.WriteLine("Nincs ilyen termék!");
                return;
            }

            product.Quantity += amount;
            Console.WriteLine($"#{product.Id} {product.Name} új mennyiség: {product.Quantity}");
        }

        static void DecreaseQuantity(Storage storage, int id, int amount)
        {
            var product = storage.FindById(id);

            if (product == null)
            {
                Console.WriteLine("Nincs ilyen termék!");
                return;
            }

            if (product.Quantity < amount)
            {
                Console.WriteLine("Nincs elég készlet!");
                return;
            }

            product.Quantity -= amount;
            Console.WriteLine($"#{product.Id} {product.Name} új mennyiség: {product.Quantity}");
        }

        public static void PrintTable(IEnumerable<Product> products)
        {
            Console.WriteLine("ID | Név     | Készlet");
            Console.WriteLine("------------------------");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}  | {p.Name,-7} | {p.Quantity}");
            }
        }
    }
}