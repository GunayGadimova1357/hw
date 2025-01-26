namespace ConsoleApp7.Models;

public class Car
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
}