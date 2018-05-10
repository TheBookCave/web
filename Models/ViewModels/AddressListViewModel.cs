namespace web.Models.ViewModels
{
    public class AddressListViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
    }
}