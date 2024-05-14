using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UserForRegister
    {
        [StringLength(256), Required, EmailAddress]
        public string Email { get; set; }
        [StringLength(256), Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string RoleName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}