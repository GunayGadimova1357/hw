using System;
using efproject.Data;
using efproject.Services;
using efproject.Models;

class Program
{
    static void Main()
    {
        using (var context = new GameStoreContext())
        {
            context.Database.EnsureDeleted(); 
            context.Database.EnsureCreated(); 

            var userService = new UserService(context);
            var gameService = new GameService(context);
            var orderService = new OrderService(context);
            
            var genre = new Genre { Name = "RPG" };
            var platform = new Platform { Name = "PC" };
            context.Genres.Add(genre);
            context.Platforms.Add(platform);
            context.SaveChanges(); 
            
            userService.RegisterUser("John", "john@example.com", 1000);
            
            gameService.AddGame("The Witcher 3", genre.Id, platform.Id, 500);
            
            orderService.BuyGame(1, 1, 1);
        }
    }
}