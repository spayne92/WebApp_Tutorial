using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<IDutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                // Custom logging will come from DutchTreat namespace.
                _logger.LogInformation("GetAllProducts was called.");

                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            } 
            catch (Exception ex)
            {
                _logger.LogInformation($"GetAllProducts failed: {ex}");

                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try 
            { 
                return _ctx.Products
                    .Where(p => p.Category == category)
                    .ToList();
            } 
            catch (Exception ex)
            {
                _logger.LogInformation($"GetProductsByCategory failed: {ex}");

                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                return _ctx.Orders
                    .Include(o => o.Items) // Includes sub-items. Excluded by default.
                    .ThenInclude(i => i.Product) // Includes sub-items' sub-items.
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"GetAllOrders failed: {ex}");

                return null;
            }
        }
        public Order GetOrderById(int id)
        {
            try
            {
                return _ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"GetAllOrders failed: {ex}");

                return null;
            }
        }

        public bool SaveAll()
        {
            try
            {
                // Saves all changes made to context since previous save.
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SaveAll failed: {ex}");

                return false;
            }
        }

        public void AddEntity(object model)
        {
            // Pushes into context, but does not commit/save.
            _ctx.Add(model);
        }
    }
}
