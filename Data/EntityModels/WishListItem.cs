namespace web.Data.EntityModels
{
    public class WishListItem
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public int BookId { get; set; }
        public string DateAdded { get; set; }
    }
}