namespace ef1.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class ServiceHistory
{
    public int Id { get; set; }

    [Required]
    public int CarId { get; set; }
    public Car Car { get; set; }

    [Required]
    public DateTime ServiceDate { get; set; }

    [Required, MaxLength(255)]
    public string Description { get; set; }
}