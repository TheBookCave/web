using System.Collections.Generic;

namespace web.Models.ViewModels
{
  public class BookGenreViewModel
  {
    public int BookId { get; set; }
    public List<string> BookGenres { get; set; }
  }
}
