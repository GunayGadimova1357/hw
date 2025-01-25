using System;
using System.Collections.Generic;

namespace TransportPark
{

    public class Transport
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int MaxSpeed { get; set; }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Type: {Type}, Brand: {Brand}, Model: {Model}, Year: {Year}, Max Speed: {MaxSpeed} km/h");
        }

        public virtual void Move()
        {
            Console.WriteLine($"{Type} starts moving.");
        }
    }
    
    public class Car : Transport
    {
        public string FuelType { get; set; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Fuel Type: {FuelType}");
        }

        public override void Move()
        {
            Console.WriteLine($"Car {Brand} {Model} is driving on the road at up to {MaxSpeed} km/h.");
        }
    }
    
    public class Truck : Transport
    {
        public double LoadCapacity { get; set; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Load Capacity: {LoadCapacity} tons");
        }

        public override void Move()
        {
            Console.WriteLine($"Truck {Brand} {Model} is transporting goods.");
        }
    }
    
    public class Bike : Transport
    {
        public bool HasSidecar { get; set; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Has Sidecar: {(HasSidecar ? "Yes" : "No")}");
        }

        public override void Move()
        {
            Console.WriteLine($"Bike {Brand} {Model} is racing down the road.");
        }
    }
    
    public class Bus : Transport
    {
        public int PassengerCapacity { get; set; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Passenger Capacity: {PassengerCapacity}");
        }

        public override void Move()
        {
            Console.WriteLine($"Bus {Brand} {Model} is carrying passengers.");
        }
    }

    class Program
    {
        static List<Transport> transportPark = new List<Transport>();

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add a transport vehicle");
                Console.WriteLine("2. Show all transport vehicles");
                Console.WriteLine("3. Start a transport vehicle");
                Console.WriteLine("4. Remove a transport vehicle");
                Console.WriteLine("5. Filter transport by type");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTransport();
                        break;
                    case "2":
                        ShowAllTransport();
                        break;
                    case "3":
                        StartTransport();
                        break;
                    case "4":
                        RemoveTransport();
                        break;
                    case "5":
                        FilterTransport();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void AddTransport()
        {
            Console.WriteLine("Choose transport type: 1. Car 2. Truck 3. Bike 4. Bus");
            string typeChoice = Console.ReadLine();
            Transport transport = null;

            switch (typeChoice)
            {
                case "1":
                    transport = new Car { Type = "Car" };
                    Console.Write("Enter fuel type: ");
                    ((Car)transport).FuelType = Console.ReadLine();
                    break;
                case "2":
                    transport = new Truck { Type = "Truck" };
                    Console.Write("Enter load capacity (tons): ");
                    ((Truck)transport).LoadCapacity = double.Parse(Console.ReadLine());
                    break;
                case "3":
                    transport = new Bike { Type = "Bike" };
                    Console.Write("Does it have a sidecar? (true/false): ");
                    ((Bike)transport).HasSidecar = bool.Parse(Console.ReadLine());
                    break;
                case "4":
                    transport = new Bus { Type = "Bus" };
                    Console.Write("Enter passenger capacity: ");
                    ((Bus)transport).PassengerCapacity = int.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            Console.Write("Enter brand: ");
            transport.Brand = Console.ReadLine();
            Console.Write("Enter model: ");
            transport.Model = Console.ReadLine();
            Console.Write("Enter year of production: ");
            transport.Year = int.Parse(Console.ReadLine());
            Console.Write("Enter max speed: ");
            transport.MaxSpeed = int.Parse(Console.ReadLine());

            transportPark.Add(transport);
            Console.WriteLine("Transport added successfully.");
        }

        static void ShowAllTransport()
        {
            if (transportPark.Count == 0)
            {
                Console.WriteLine("No transport vehicles available.");
                return;
            }

            for (int i = 0; i < transportPark.Count; i++)
            {
                Console.WriteLine($"\nTransport #{i + 1}:");
                transportPark[i].ShowInfo();
            }
        }

        static void StartTransport()
        {
            Console.Write("Enter the number of the transport to start: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= transportPark.Count)
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            transportPark[index].Move();
        }

        static void RemoveTransport()
        {
            Console.Write("Enter the number of the transport to remove: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= transportPark.Count)
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            transportPark.RemoveAt(index);
            Console.WriteLine("Transport removed successfully.");
        }

        static void FilterTransport()
        {
            Console.Write("Enter transport type to filter (Car, Truck, Bike, Bus): ");
            string type = Console.ReadLine();

            foreach (var transport in transportPark)
            {
                if (transport.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                {
                    transport.ShowInfo();
                }
            }
        }
    }
}