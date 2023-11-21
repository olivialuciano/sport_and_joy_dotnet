using Microsoft.EntityFrameworkCore;
using sport_and_joy_back_dotnet.Entities;
using System.Text.RegularExpressions;

namespace sport_and_joy_back_dotnet.Data
{
    public class SportContext : DbContext
    {

        public SportContext(DbContextOptions<SportContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Field> Fields { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User usr1 = new User()
            {
                Id = 1,
                FirstName = "Olivia",
                LastName = "Luciano",
                Password = "oliolioli",
                Email = "olilu@gmail.com",
                Role = Erole.ADMIN
            };
            User usr2 = new User()
            {
                Id = 2,
                FirstName = "Vicky",
                LastName = "Svaikauskas",
                Password = "India123",
                Email = "visvaik@gmail.com",
                Role = Erole.OWNER

            };
            User usr3 = new User()
            {
                Id = 3,
                FirstName = "Luci",
                LastName = "Ansaldi",
                Password = "lululu",
                Email = "luans@gmail.com",
                Role = Erole.PLAYER

            };
            Field fie1 = new Field()
            {
                Id = 1,
                Name = "futbol lindo",
                Location = "Paraguay 1950",
                Description = "Cancha de f5 para todos.",
                LockerRoom = false,
                Bar = true,
                Sport = Esport.FOOTALL,
                UserId = usr2.Id
            };
            Field fie2 = new Field()
            {
                Id = 2,
                Name = "voley lindo",
                Location = "Corrientes 1950",
                Description = "Cancha de voley para todos.",
                LockerRoom = true,
                Bar = false,
                Sport = Esport.VOLLEY,
                UserId = usr2.Id
            };
            Field fie3 = new Field()
            {
                Id = 3,
                Name = "tenis lindo",
                Location = "Entre Rios 1950",
                Description = "Cancha de tenis para todos.",
                LockerRoom = true,
                Bar = true,
                Sport = Esport.TENNIS,
                UserId = usr2.Id
            };
            Reservation res1 = new Reservation()
            {
                Id = 1,
                Date = new DateTime(2023, 12, 12),
                UserId = usr3.Id,
                FieldId = fie1.Id,
            };
            Reservation res2 = new Reservation()
            {
                Id = 2,
                Date = new DateTime(2023, 12, 22),
                UserId = usr3.Id,
                FieldId = fie2.Id,
            };
            Reservation res3 = new Reservation()
            {
                Id = 3,
                Date = new DateTime(2023, 12, 31),
                UserId = usr3.Id,
                FieldId = fie3.Id,
            };


            modelBuilder.Entity<User>()
                .HasMany(u => u.Fields)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Field>()
                .HasMany(f => f.Reservations)
                .WithOne(r => r.Field)
                .HasForeignKey(r => r.FieldId)
                .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<User>().HasData(usr1, usr2, usr3);
            modelBuilder.Entity<Field>().HasData(fie1, fie2, fie3);
            modelBuilder.Entity<Reservation>().HasData(res1, res2, res3);

            base.OnModelCreating(modelBuilder);


        }

    }
}

