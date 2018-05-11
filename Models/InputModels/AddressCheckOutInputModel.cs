using System.Collections.Generic;
using web.Models.ViewModels;

namespace web.Models.InputModels
{
    public class AddressCheckOutInputModel
    {
        public AddressListViewModel ShippingAddressOption { get; set; }
        public AddressListViewModel BillingAddressOption { get; set; }
        public AddressListViewModel ShippingAddressInput { get; set; }
        public AddressListViewModel BillingAddressInput { get; set; }

        public List<AddressListViewModel> AllUserAddresses { get; set; }
    }
}