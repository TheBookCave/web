
using web.Data;

using Microsoft.AspNetCore.Identity;

namespace web.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int FavoriteBookId { get; set; }
    public int PrimaryAddressId { get; set; }
    public string UserPhotoLocation { get; set; }
  }
}