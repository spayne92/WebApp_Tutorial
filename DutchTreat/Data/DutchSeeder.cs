using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly DutchContext _ctx;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void Seed()
        {
            // Runs previous migrations, like basic Order seeding in SeedData.
            _ctx.Database.EnsureCreated();

            // Queries DB to check for existing Product records.
            if (!_ctx.Products.Any())
            {
                // No Products found, needs to seed from sample JSON data.
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                // Orders exist from previous seed done with HasData in DutchContext.
                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _ctx.SaveChanges();
            }
        }
    }
}
