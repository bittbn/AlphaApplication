using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AlphaApplication.Models
{
    public class LoginModel
    {
        [Display(Name = "Логин"), StringLength(50), Required]
        public string Login { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), StringLength(50), Required]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "Имя"), StringLength(50), Required]
        public string Name { get; set; }

        [Display(Name = "Фамилия"), StringLength(50), Required]
        public string Surname { get; set; }

        [Display(Name = "Логин"), StringLength(50), Required]
        public string Login { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), StringLength(50), Required]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают"), StringLength(50), Required]
        public string ConfirmPassword { get; set; }
    }
}