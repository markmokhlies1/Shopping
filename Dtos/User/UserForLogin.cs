using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UserForLogin
    {
        [StringLength(256), Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NormalizedUserName { get; set; }
    }
}