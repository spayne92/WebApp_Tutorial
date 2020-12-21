using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class ContactViewModel
    {
        [Required] // Validates property is provided.
        [MinLength(5)] // Validates string is above certain length.
        public string Name { get; set; }
        [Required]
        [EmailAddress] // Validates string is 'valid' email.
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Too Long")] // Overwrites default error message.
        public string Message { get; set; }
    }
}
