namespace web.Data.EntityModels
{
  public class Order
  {
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public int BillingAddressId { get; set; }
    public int ShippingAddressId { get; set; }
    public string Status { get; set; }
    public string OrderDate { get; set; }
    public string ShippingDate { get; set; }
    public string TrackingNumber { get; set; }
    public decimal PurchaseAmount { get; set; }
  }
}