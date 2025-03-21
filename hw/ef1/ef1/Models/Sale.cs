namespace ef1.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Sale
{
    public int Id { get; set; }

    [Required]
    public int CarId { get; set; }
    public Car Car { get; set; }

    [Required]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public DateTime SaleDate { get; set; }
}