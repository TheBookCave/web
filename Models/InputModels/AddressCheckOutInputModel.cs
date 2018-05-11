using System.Collections.Generic;
using web.Models.ViewModels;

namespace web.Models.InputModels
{
    public class AddressCheckOutInputModel
    {
        public int ShippingAddressOption { get; set; }
        public int BillingAddressOption { get; set; }
        public AddressInputModel ShippingAddressInput { get; set; }
        public AddressInputModel BillingAddressInput { get; set; }

        public List<AddressListViewModel> AllUserAddresses { get; set; }
    }
}