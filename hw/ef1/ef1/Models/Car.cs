namespace ef1.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Car
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Brand { get; set; }

    [Required, MaxLength(50)]
    public string Model { get; set; }

    public int Year { get; set; }

    [Required]
    public decimal Price { get; set; }

    public ICollection<ServiceHistory> ServiceHistories { get; set; }
}

