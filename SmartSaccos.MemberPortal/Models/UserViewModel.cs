using SmartSaccos.Domains.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.MemberPortal.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoggedInUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string TokenString { get; set; }
        public string OtherNames { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public bool Succeeded { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string MemberNumber { get; set; }
        public MemberStatus Status { get; set; }
        public bool WeKnowCustomer { get; set; }
        public int MemberId { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public int CountryId { get; set; }
    }
}
