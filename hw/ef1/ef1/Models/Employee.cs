namespace ef1.Models;

using System.ComponentModel.DataAnnotations;
public class Employee
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public string Position { get; set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}