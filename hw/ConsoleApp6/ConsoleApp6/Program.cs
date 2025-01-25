using System;


interface ICalculatorOperation
{
    double Execute(double a, double b);
    string Name { get; }
}

class Addition : ICalculatorOperation
{
    public double Execute(double a, double b) => a + b;
    public string Name => "Addition";
}

class Subtraction : ICalculatorOperation
{
    public double Execute(double a, double b) => a - b;
    public string Name => "Subtraction";
}

class Multiplication : ICalculatorOperation
{
    public double Execute(double a, double b) => a * b;
    public string Name => "Multiplication";
}

class Division : ICalculatorOperation
{
    public double Execute(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Division by zero is not allowed.");
        return a / b;
    }
    public string Name => "Division";
}

class Program
{
    static List<ICalculatorOperation> operations = new List<ICalculatorOperation>
    {
        new Addition(),
        new Subtraction(),
        new Multiplication(),
        new Division()
    };

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nAvailable Operations:");
            for (int i = 0; i < operations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {operations[i].Name}");
            }
            Console.WriteLine($"{operations.Count + 1}. Add a new operation");
            Console.WriteLine($"{operations.Count + 2}. Exit");

            Console.Write("\nSelect an operation: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1  || choice > operations.Count + 2)
            {
                Console.WriteLine("Invalid selection. Try again.");
                continue;
            }

            if (choice == operations.Count + 2)
                break;

            if (choice == operations.Count + 1)
            {
                AddNewOperation();
                continue;
            }

            try
            {
                Console.Write("Enter the first number: ");
                double a = double.Parse(Console.ReadLine());

                Console.Write("Enter the second number: ");
                double b = double.Parse(Console.ReadLine());

                double result = operations[choice - 1].Execute(a, b);
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                LogError(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid input.");
                LogError("Invalid input.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                LogError(ex.Message);
            }
        }
    }

    static void AddNewOperation()
    {
        Console.Write("Enter the name of the operation: ");
        string name = Console.ReadLine();

        Console.Write("Enter a lambda expression for the calculation (e.g., a+b): ");
        string lambda = Console.ReadLine();

        try
        {
            Func<double, double, double> customOperation = (a, b) =>
            {
                string expression = lambda.Replace("a", a.ToString()).Replace("b", b.ToString());
                return (double)new System.Data.DataTable().Compute(expression, null);
            };

            operations.Add(new CustomOperation(name, customOperation));
            Console.WriteLine($"Operation \"{name}\" has been added.");
        }
        catch
        {
            Console.WriteLine("Failed to add the operation.");
        }
    }

    static void LogError(string message)
    {
        File.AppendAllText("errors.log", $"{DateTime.Now}: {message}\n");
    }
}

class CustomOperation : ICalculatorOperation
{
    private readonly Func<double, double, double> _operation;

    public CustomOperation(string name, Func<double, double, double> operation)
    {Name = name;
        _operation = operation;
    }

    public double Execute(double a, double b) => _operation(a, b);
    public string Name { get; }
}