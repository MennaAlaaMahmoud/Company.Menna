﻿using System.ComponentModel.DataAnnotations;

namespace Company.Menna.PL.Dtos
{
    public class SingInDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)] // ******
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
