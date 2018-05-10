using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using web.Data.EntityModels;

namespace web.Models.InputModels
{
    public class AuthorInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}