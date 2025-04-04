using System.ComponentModel.DataAnnotations;

namespace Company.Menna.PL.Dtos
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)] // ******
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;""<>,.?/-]).{8,}$", ErrorMessage = "Password must contain at least 8 characters, including an uppercase letter, a lowercase letter, a digit, and a special character !!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)] // ******
        [Required(ErrorMessage = "Confirm Password is Required !!")]
        [Compare(nameof(NewPassword), ErrorMessage = " Confirm Password do not match the Password !!")]
        public string ConfirmPassword { get; set; }

    }
}
