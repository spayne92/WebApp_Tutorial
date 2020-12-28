using System;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        [Required] // Uses Model Data Annotations for validation.
        [MinLength(4) ]
        public string OrderNumber { get; set; }
    }
}
