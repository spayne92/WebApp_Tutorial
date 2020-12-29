using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    // Designates as sub-controller of OrdersController
    [Route("/api/orders/{orderId}/items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<ProductsController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.OrderItems.Get() failed: {ex}");
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                    if (item != null)
                    {
                        return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders.OrderItems.Get() failed: {ex}");
            }
            return BadRequest();
        }

        // Needs POST and DELETE methods implemented for full CRUD. 
        // Left as excercise and references API specific course for reference. 
    }
}
