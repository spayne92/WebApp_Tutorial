using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data;
using DutchTreat.ViewModels;
using DutchTreat.Data.Entities;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public OrdersController(IDutchRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.Get() (failed: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        // Extends URI with another property and also defines type.
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound(); // Helpers provided by IActionResult
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.Get(id) (failed: {ex}");
                return BadRequest($"Failed to get order: {id}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            // FromBody attribute overrides default of grabbing model from URL text.
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = new Order()
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                        Id = model.OrderId
                    };

                    // If date not specified, overwrite it.
                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    } 

                    // Takes data and attaches to context.
                    _repository.AddEntity(newOrder);

                    // Saves changes made in context.
                    if (_repository.SaveAll())
                    {
                        var returnModel = new OrderViewModel()
                        {
                            OrderDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber,
                            OrderId = newOrder.Id
                        };

                        // HTTP requires POST to return 'Created' 201 response if object created.
                        return Created($"/api/order/{returnModel.OrderId}", returnModel);
                        // URI sent back in case consumer needs to keep track whether they have most recent.
                    }
                }
                else
                {
                    // Exposes errors in model state from the data request to sender.
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.Post(order) failed: {ex}");
            }

            return BadRequest($"Failed to save a new order");
        }
    }
}
