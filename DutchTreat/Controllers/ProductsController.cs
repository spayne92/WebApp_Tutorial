using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DutchTreat.Data;
using DutchTreat.Data.Entities;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")] // Sets route of accessing controller
    [ApiController] // Annotation for validation and documentation tools.
    [Produces("application/json")] // Annotation for validation and documenation tools.
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)] // Annotations for validation and documenation tools.
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            // ActionResult used instead of concrete JsonResult for genericness.
            //      MVC handles serializing to requested types this way, instead of enforcing one.
            // Concrete ActionResult used instead of IActionResult with a type template given.
            //      Done basically only for the purpose of API auto-documenting with Swagger, etc.
            // * Only relevant for public APIs. Private ones better off using generic IActionResult.

            try
            {
                // Creates generic 'Ok' state of requested datatype, default JSON.
                return Ok(_repository.GetAllProducts());
                // If concrete types used (i.e. not IEnumerable), MVC 6+ can imply the
                //      the 'Ok' response without it be implicitly used.
            }
            catch (Exception ex)
            {
                _logger.LogError($"Products.Get() (failed: {ex}");
                return BadRequest("Failed to get products");
            }
        }

    }
}
