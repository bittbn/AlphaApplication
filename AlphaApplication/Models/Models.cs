using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlphaApplication.Models
{
    [Table("User")]
    public class User
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Имя"), StringLength(50), Required]
        public string Name { get; set; }

        [Display(Name = "Фамилия"), StringLength(50), Required]
        public string Surname { get; set; }

        [Display(Name = "Логин"), StringLength(50), Required]
        public string Login { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), StringLength(50), Required]
        public string Password { get; set; }

        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }

    [Table("Role")]
    public class Role
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Должность"), StringLength(50), Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    [Table("Room")]
    public class Room
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Аудитория"), StringLength(50), Required]
        public string Name { get; set; }

        [Display(Name = "Описание"), StringLength(500), Required]
        public string Description { get; set; }

        [Display(Name = "Количество кресел"), Required]
        public int Seats { get; set; }

        [Display(Name = "Маркерная доска"), Required]
        public bool Board { get; set; }

        [Display(Name = "Проектор"), Required]
        public bool Projector { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }

    [Table("Reservation")]
    public class Reservation
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Дата"), DataType(DataType.Date), Required]
        public DateTime Date { get; set; }

        [Display(Name = "Начало"), DataType(DataType.Time), Required]
        public TimeSpan TimeStart { get; set; }

        [Display(Name = "Конец"), DataType(DataType.Time), Required]
        public TimeSpan TimeEnd { get; set; }

        [Required]
        public bool Allow { get; set; } = false;

        public int? UserId { get; set; }

        public int? RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}