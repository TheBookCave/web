namespace web.Models.InputModels
{
  public class OrderItemInputModel
  {
    public int Id { get; set; }
    public decimal ItemDiscount { get; set; }
    public decimal ItemPrice { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public int BookId { get; set; }
  }
}