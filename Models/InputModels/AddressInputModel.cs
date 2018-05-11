using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
    public class AddressInputModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [StringLength(15)]
        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string ZipCode { get; set; }
    }
}