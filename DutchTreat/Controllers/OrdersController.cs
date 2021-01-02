using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    [Route("/api/[Controller]")]        // [Authorize] to ALL actions.
    //[Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                // Reduces load when given the value 'false' from API requests.
                var results = _repository.GetAllOrders(includeItems);

                // Single entity mapping configurations also provide mappings for collections of those entities.
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.Get() (failed: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        // Extends URI with another property and also defines type.
        [HttpGet("{id:int}")]           // Action level JWT Authentication
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null)
                {
                    // Maps Order entity to OrderViewModel to give to request.
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                    // Requires existing type map configuration for the types.
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
                    // Maps OrderViewModel from request to Order entity for operations.
                    var orderEntity = _mapper.Map<OrderViewModel, Order>(model);

                    // If date not specified, overwrite it.
                    if (orderEntity.OrderDate == DateTime.MinValue)
                    {
                        orderEntity.OrderDate = DateTime.Now;
                    } 

                    // Takes data and attaches to context.
                    _repository.AddEntity(orderEntity);

                    // Saves changes made in context.
                    if (_repository.SaveAll())
                    {
                        // Maps Order entity back to OrderViewModel for response request.
                        var returnModel = _mapper.Map<Order, OrderViewModel>(orderEntity);

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
