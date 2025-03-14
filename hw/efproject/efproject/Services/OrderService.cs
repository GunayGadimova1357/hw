namespace efproject.Services;
using System;
using System.Linq;
using efproject.Models;
using efproject.Data;



    public class OrderService
    {
        private readonly GameStoreContext _context;

        public OrderService(GameStoreContext context)
        {
            _context = context;
        }

        public void BuyGame(int userId, int gameId, int quantity)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId);

            if (user == null)
            {
                Console.WriteLine("Error: User not found.");
                return;
            }

            if (game == null)
            {
                Console.WriteLine("Error: Game not found.");
                return;
            }

            decimal totalCost = game.Price * quantity;

            if (user.Balance < totalCost)
            {
                Console.WriteLine("Error: Insufficient funds.");
                return;
            }
            
            var order = new Order { UserId = userId, TotalAmount = totalCost };
            _context.Orders.Add(order);
            _context.SaveChanges();
            
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                GameId = gameId,
                Quantity = quantity,
                TotalPrice = totalCost
            };

            _context.OrderItems.Add(orderItem);
            user.Balance -= totalCost;
            _context.SaveChanges();

            Console.WriteLine($"Game '{game.Title}' purchased successfully.");
        }
    }
