using System.ComponentModel.DataAnnotations;

namespace web.Models.InputModels
{
  public class LoginInputModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public bool Remember { get; set; }
  }
}