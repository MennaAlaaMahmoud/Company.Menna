using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Menna.PL.Dtos
{
    public class SingUpDto
    {
        [Required(ErrorMessage ="UserName is Required !!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is Required !!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required !!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)] // ******
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;""<>,.?/-]).{8,}$", ErrorMessage = "Password must contain at least 8 characters, including an uppercase letter, a lowercase letter, a digit, and a special character !!")]
        public string Password { get; set; }

        [DataType(DataType.Password)] // ******
        [Required(ErrorMessage = "Confirm Password is Required !!")]
        [Compare(nameof (Password), ErrorMessage = " Confirm Password do not match the Password !!")]
        public string ConfirmPassword { get; set; }


        public bool IsAgree { get; set; }

    }
}
