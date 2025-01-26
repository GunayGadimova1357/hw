using ConsoleApp7.Models;
using ConsoleApp7.Utils;

namespace ConsoleApp7;

static class Program
{
    static IRepository<Showroom> showroomRepository = new JsonRepository<Showroom>("showrooms.json");
    static IRepository<User> userRepository = new JsonRepository<User>("users.json");
    static IRepository<Sale> saleRepository = new JsonRepository<Sale>("sales.json");

    static void Main(string[] args)
    {
        Console.WriteLine("Car Showroom!");
        while (true)
        {
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            string choice = Console.ReadLine();

            if (choice == "1") Login();
            else if (choice == "2") Register();
            else if (choice == "3") break;
            else Console.WriteLine("Invalid input.");
        }
    }

    static void Register()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        if (userRepository.GetAll().Any(u => u.Username == username))
        {
            Console.WriteLine("Username already exists.");
            return;
        }

        userRepository.Add(new User { Username = username, Password = password });
        Console.WriteLine("Registration successful!");
    }

    static void Login()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        var user = userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        Console.WriteLine("Login successful!");
        UserMenu(user);
    }

    static void UserMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("1. Create Showroom");
            Console.WriteLine("2. Manage Showroom");
            Console.WriteLine("3. View Statistics");
            Console.WriteLine("4. Logout");
            string choice = Console.ReadLine();

            if (choice == "1") CreateShowroom(user);
            else if (choice == "2") ManageShowroom(user);
            else if (choice == "3") ShowStatistics();
            else if (choice == "4") break;
            else Console.WriteLine("Invalid input.");
        }
    }

    static void CreateShowroom(User user)
    {
        Console.WriteLine("Enter showroom name:");
        string name = Console.ReadLine();
        Console.WriteLine("Enter car capacity:");
        int capacity = int.Parse(Console.ReadLine());

        showroomRepository.Add(new Showroom { Name = name, CarCapacity = capacity });
        Console.WriteLine("Showroom created successfully.");
    }

    static void ManageShowroom(User user)
    {
        foreach (var CurrentShowroom in showroomRepository.GetAll())
        {
            Console.WriteLine($"ID: {CurrentShowroom.Id}, Name: {CurrentShowroom.Name}, Cars: {CurrentShowroom.CarCount}/{CurrentShowroom.CarCapacity}");
        }

        Console.WriteLine("Enter the ID of the showroom to manage:");
        Guid showroomId = Guid.Parse(Console.ReadLine());
        var showroom = showroomRepository.Get(showroomId);

        if (showroom == null)
        {
            Console.WriteLine("Showroom not found.");
            return;
        }

        while (true)
        {
            Console.WriteLine("1. Add Car");
            Console.WriteLine("2. Remove Car");
            Console.WriteLine("3. Read Car");
            Console.WriteLine("4. Update Car");
            Console.WriteLine("5. Sell Car");
            Console.WriteLine("6. Back");
            string choice = Console.ReadLine();

            if (choice == "1") AddCarToShowroom(showroom);
            else if (choice == "2") RemoveCarFromShowroom(showroom);
            else if (choice == "3") ReadCar(showroom);
            else if (choice == "4") UpdateCar(showroom);
            else if (choice=="5")  SellCar(showroom, user);
            else if (choice == "6") break;
            else Console.WriteLine("Invalid input.");
        }
    }

    static void AddCarToShowroom(Showroom showroom)
    {
        Console.WriteLine("Enter car make:");
        string make = Console.ReadLine();
        Console.WriteLine("Enter car model:");
        string model = Console.ReadLine();
        Console.WriteLine("Enter car year:");
        int year = int.Parse(Console.ReadLine());

        showroom.AddCar(new Car { Make = make, Model = model, Year = year });
        showroomRepository.Add(showroom);
        Console.WriteLine("Car added successfully.");
    }

    static void RemoveCarFromShowroom(Showroom showroom)
    {
        foreach (var car in showroom.Cars)
        {
            Console.WriteLine($"ID: {car.Id}, Make: {car.Make}, Model: {car.Model}, Year: {car.Year}");
        }

        Console.WriteLine("Enter the ID of the car to remove:");
        Guid carId = Guid.Parse(Console.ReadLine());

        if (showroom.RemoveCar(carId))
        {
            showroomRepository.Add(showroom);
            Console.WriteLine("Car removed successfully.");
        }
        else
        {
            Console.WriteLine("Car not found.");
        }
    }
        static void ReadCar(Showroom showroom)
    {
        Console.WriteLine("Cars id in showroom:");
        foreach (var car1 in showroom.Cars)
        {
            Console.WriteLine($"ID: {car1.Id}");
        }
        
        Console.WriteLine("Enter the ID of the car to read:");
        Guid carId = Guid.Parse(Console.ReadLine());
        var car = showroom.Cars.FirstOrDefault(c => c.Id == carId);
    
        if (car == null)
        {
            Console.WriteLine("Car not found.");
            return;
        }
    
        Console.WriteLine($"Car Details:");
        Console.WriteLine($"ID: {car.Id}");
        Console.WriteLine($"Make: {car.Make}");
        Console.WriteLine($"Model: {car.Model}");
        Console.WriteLine($"Year: {car.Year}");
    }
    
    static void UpdateCar(Showroom showroom)
    {
        Console.WriteLine("Cars in showroom:");
        foreach (var car1 in showroom.Cars)
        {
            Console.WriteLine($"ID: {car1.Id}, Make: {car1.Make}, Model: {car1.Model}, Year: {car1.Year}");
        }
    
        Console.WriteLine("Enter the ID of the car to update:");
        Guid carId = Guid.Parse(Console.ReadLine());
        var car = showroom.Cars.FirstOrDefault(c => c.Id == carId);
    
        if (car == null)
        {
            Console.WriteLine("Car not found.");
            return;
        }
    
        Console.WriteLine("Updating car details. Leave blank to keep the current value.");
        Console.WriteLine($"Current Make: {car.Make}");
        Console.Write("Enter new Make: ");
        string newMake = Console.ReadLine();
        if (!string.IsNullOrEmpty(newMake)) car.Make = newMake;
    
        Console.WriteLine($"Current Model: {car.Model}");
        Console.Write("Enter new Model: ");
        string newModel = Console.ReadLine();
        if (!string.IsNullOrEmpty(newModel)) car.Model = newModel;
    
        Console.WriteLine($"Current Year: {car.Year}");
        Console.Write("Enter new Year: ");
        string newYear = Console.ReadLine();
        if (int.TryParse(newYear, out int year) && year > 0) car.Year = year;
    
        Console.WriteLine("Car details updated successfully.");
    }
    
    static void SellCar(Showroom showroom, User user)
    {
        Console.WriteLine("Cars in showroom:");
        foreach (var car1 in showroom.Cars)
        {
            Console.WriteLine($"ID: {car1.Id}, Make: {car1.Make}, Model: {car1.Model}, Year: {car1.Year}");
        }
    
        Console.WriteLine("Enter the ID of the car to sell:");
        Guid carId = Guid.Parse(Console.ReadLine());
        var car = showroom.Cars.FirstOrDefault(c => c.Id == carId);
    
        if (car == null)
        {
            Console.WriteLine("Car not found.");
            return;
        }
    
        if (string.IsNullOrEmpty(car.Make))
        {
            Console.WriteLine("Car make is not specified. Sale cannot be recorded.");
            return;
        }
    
        Console.WriteLine("Enter sell price:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Invalid price format.");
            return;
        }
    
        Console.WriteLine("Enter the sell date (yyyy/MM/dd):");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime sellDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }
    
        saleRepository.Add(new Sale
        {
            ShowroomId = showroom.Id,
            CarId = car.Id,
            UserId = user.Id,
            SaleDate = sellDate,
            SalePrice = price,
            CarMake = car.Make
        });
    
        showroom.RemoveCar(carId);
        showroom.SalesCount++;
    
        Console.WriteLine("Car sold successfully.");
    }

    static void ShowStatistics()
    {
        Console.WriteLine("1. Sales by Showroom");
        Console.WriteLine("2. Sales by Car Make");
        Console.WriteLine("3. Back");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            foreach (var showroom in showroomRepository.GetAll())
            {
                var showroomSales = saleRepository.GetAll().Where(s => s.ShowroomId == showroom.Id);
                Console.WriteLine($"Showroom {showroom.Name} has {showroomSales.Count()} sales.");
            }
        }
        else if (choice == "2")
        {
            Console.WriteLine("Enter car make:");
            string make = Console.ReadLine();
            var salesByMake = saleRepository.GetAll().Where(s => s.CarMake.Equals(make, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"Total sales for {make}: {salesByMake.Count()}");
        }
        else if (choice == "3")
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}