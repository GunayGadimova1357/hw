namespace ef1.Models;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, Phone]
    public string Phone { get; set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
