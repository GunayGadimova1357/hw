namespace efproject.Services;

using System;
using efproject.Models;
using efproject.Data;

public class UserService
{
    private readonly GameStoreContext _context;

    public UserService(GameStoreContext context)
    {
        _context = context;
    }

    public void RegisterUser(string name, string email, decimal initialBalance)
    {
        var user = new User { Name = name, Email = email, Balance = initialBalance };
        _context.Users.Add(user);
        _context.SaveChanges();
        Console.WriteLine($"User {name} has been registered.");
    }

}