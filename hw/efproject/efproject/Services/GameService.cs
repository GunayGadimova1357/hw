namespace efproject.Services;

using System;
using System.Linq;
using efproject.Models;
using efproject.Data;

public class GameService
{
    private readonly GameStoreContext _context;

    public GameService(GameStoreContext context)
    {
        _context = context;
    }

    public void AddGame(string title, int genreId, int platformId, decimal price)
    {
        var game = new Game { Title = title, GenreId = genreId, PlatformId = platformId, Price = price };
        _context.Games.Add(game);
        _context.SaveChanges();
        Console.WriteLine($"Game {title} added.");
    }

    public void ShowGames()
    {
        foreach (var game in _context.Games)
        {
            Console.WriteLine($"{game.Id}. {game.Title} - Price: {game.Price}");
        }
    }

    
}