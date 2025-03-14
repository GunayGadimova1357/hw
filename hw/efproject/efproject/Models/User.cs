using System.Transactions;

namespace efproject.Models;

using System;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public List<Order> Orders { get; set; }
}