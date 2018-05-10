using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace web.Models.InputModels
{
    public class UserEditInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FavoriteBookId { get; set; }

        public int PrimaryAddressId {get; set; }

        public IFormFile UserPhoto {get; set; }
    }
}