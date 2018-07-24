using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AlphaApplication.Models
{
    public class AlphaContext : DbContext
    {
        public AlphaContext() : base("DefaultConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Room)
                .HasForeignKey(e => e.RoomId);
        }

        public class MyDbInitializer : DropCreateDatabaseAlways<AlphaContext>
        {
            protected override void Seed(AlphaContext db)
            {
                Role manager = new Role { Id = 1, Name = "Офисный менеджер" };
                Role employee = new Role { Id = 2, Name = "Сотрудник" };

                db.Roles.Add(employee);
                db.Roles.Add(manager);
                db.Users.Add(new User
                {
                    Name = "Никита",
                    Surname = "Бирюков",
                    Login = "admin",
                    Password = "admin",
                    Role = manager
                });
                base.Seed(db);
            }
        }
    }
}