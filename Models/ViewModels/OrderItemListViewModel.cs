namespace web.Models.ViewModels
{
    public class OrderItemListViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ItemDiscount { get; set;}
        public decimal ItemTotalPrice { get; set; }

    }
}