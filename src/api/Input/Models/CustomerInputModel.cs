using System.ComponentModel.DataAnnotations;

namespace api.Input.Models
{
    public class CustomerInputModel
    {
        public string Honorific { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}