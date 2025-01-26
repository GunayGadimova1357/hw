namespace ConsoleApp7.Models;

public class Showroom
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public List<Car> Cars { get; set; } = new List<Car>();
    public int CarCapacity { get; set; }
    public int CarCount => Cars.Count;
    public int SalesCount { get; set; }

    public bool AddCar(Car car)
    {
        if (Cars.Count < CarCapacity)
        {
            Cars.Add(car);
            return true;
        }
        return false;
    }

    public bool RemoveCar(Guid carId)
    {
        var car = Cars.FirstOrDefault(c => c.Id == carId);
        if (car != null)
        {
            Cars.Remove(car);
            return true;
        }
        return false;
    }
}