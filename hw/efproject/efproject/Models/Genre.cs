namespace efproject.Models;
using System.Collections.Generic;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Game> Games { get; set; } = new List<Game>();
}