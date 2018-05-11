using System.Collections.Generic;

namespace web.Models.ViewModels
{
  public class OrderConfirmationViewModel
  {
    public OrderDetailViewModel Order { get; set; }
    public List<AddressListViewModel> Addresses { get; set; }
    public List<OrderItemListViewModel> OrderItems { get; set; }
  }
}