﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data;

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
    }
}
