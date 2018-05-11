using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
  public class GenreInputModel
  {
    [Required]
    public string Name { get; set; }
  }
}