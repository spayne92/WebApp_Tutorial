using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;
        private readonly DutchContext _ctx;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            // Runs previous migrations, like basic Order seeding in SeedData migration.
            _ctx.Database.EnsureCreated();

            // Manages users through the identity system instead of directly through DbContext.
            StoreUser user = await _userManager.FindByEmailAsync("spayne@mst.edu");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Scott",
                    LastName = "Payne",
                    Email = "spayne@mst.edu",
                    UserName = "spayne"
                };

                // Allows configuration of password complexity. Default is decent.
                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder.");
                }
            }

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
                    order.User = user;
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
