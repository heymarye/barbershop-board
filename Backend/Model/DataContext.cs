using Microsoft.EntityFrameworkCore;
using Backend.Helpers;
using static Backend.Model.Client;
using static Backend.Model.Hairdresser;

namespace Backend.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Client>? Clients { get; set; }
        public DbSet<Hairdresser>? Hairdressers { get; set; }
        public DbSet<Service>? Services { get; set; }
        public DbSet<Booking>? Bookings { get; set; }
        public DbSet<SpecialOffer>? SpecialOffers { get; set; }
        public DbSet<User>? UserList { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "test1", //Password is test1
                    Password = "St9tpNN2zrinRGNUgKWCy4JjZRFEorSQ0Zg3a/8m7k4=",
                },
                new User
                {
                    Id = 2,
                    Username = "test2",
                    Password = "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=", //Password is test 2
                },
                new User
                {
                    Id = 3,
                    Username = "test3",
                    Password = "6RwNz8ehCp0yZ0KkUE7i+Shy+2l7C1Eh9dT/RULwZN8=", //Password is test 3
                }
            );

            modelBuilder.Entity<Client>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Address = "123 Oak Street",
                    Mail = "john.smith@email.com",
                    Mobile = "+1 (555) 555-1234"
                },
                new Client
                {
                    Id = 2,
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Address = "456 Elm Avenue",
                    Mail = "michael.johnson@email.com",
                    Mobile = "+1 (555) 555-5678"
                },
                new Client
                {
                    Id = 3,
                    FirstName = "David",
                    LastName = "Williams",
                    Address = "789 Maple Lane",
                    Mail = "david.williams@email.com",
                    Mobile = "+1 (555) 555-2345"
                },
                new Client
                {
                    Id = 4,
                    FirstName = "Matthew",
                    LastName = "Brown",
                    Address = "101 Cedar Road",
                    Mail = "matthew.brown@email.com",
                    Mobile = "+1 (555) 555-7890"
                },
                new Client
                {
                    Id = 5,
                    FirstName = "Daniel",
                    LastName = "Davis",
                    Address = "222 Birch Street",
                    Mail = "daniel.davis@email.com",
                    Mobile = "+1 (555) 555-6789"
                }
            );

            modelBuilder.Entity<Hairdresser>().Property(p => p.Id).HasIdentityOptions(startValue: 4);
            modelBuilder.Entity<Hairdresser>().HasData(
                new Hairdresser
                {
                    Id = 1,
                    FirstName = "James",
                    LastName = "Johnson",
                    Mail = "james.johnson@barbershop.com",
                    Mobile = "+1 (555) 555-5678"
                },
                new Hairdresser
                {
                    Id = 2,
                    FirstName = "Robert",
                    LastName = "Williams",
                    Mail = "robert.williams@barbershop.com",
                    Mobile = "+1 (555) 555-2345"
                },
                 new Hairdresser
                 {
                     Id = 3,
                     FirstName = "Matthew",
                     LastName = "Davis",
                     Mail = "matthew.davis@barbershop.com",
                     Mobile = "+1 (555) 555-6789"
                 }
            );

            modelBuilder.Entity<Service>().Property(p => p.Id).HasIdentityOptions(startValue: 5);
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Title = "Haircut",
                    Description = "Haircut is performed with scissors and clippers/only scissors. Includes a hair wash and styling using a professional cosmetic products.",
                    Price = 32,
                    Duration = DateTime.Parse("00:45").ToString("HH:mm")
                },
                new Service
                {
                    Id = 2,
                    Title = "Clipper Cut",
                    Description = "Haircut is being carried out only with hair clippers. Includes hair wash and as well as styling using professional cosmetic products. Cutting with a shaver if cutting to zero is required. Haircut with Fade tchnology and sides cutting only are not included in the service.",
                    Price = 23,
                    Duration = DateTime.Parse("00:30").ToString("HH:mm")
                },
                new Service
                {
                    Id = 3,
                    Title = "Complex Service (Haircut + Beard Design)",
                    Description = "Haircut with scissors/clipper cut + beard design with trimmer and shaver.",
                    Price = 44,
                    Duration = DateTime.Parse("01:30").ToString("HH:mm")
                },
                new Service
                {
                    Id = 4,
                    Title = "Beard design (Trimmer + Shaver)",
                    Description = "Beard shape modelling with clippers/scissors, cheeks and neck contours creating by trimmer and shaver.",
                    Price = 22,
                    Duration = DateTime.Parse("00:40").ToString("HH:mm")
                },
                new Service
                {
                    Id = 5,
                    Title = "Beard design (Trimmer + Razor + Hot Towel)",
                    Description = "Beard modelling with clippers/scissors, steaming by hot towel, creating contours with shavette razor with disposable razor blades.",
                    Price = 35,
                    Duration = DateTime.Parse("01:00").ToString("HH:mm")
                },
                new Service
                {
                    Id = 6,
                    Title = "Traditional wet shaving Royal Shave",
                    Description = "Traditional face shaving. Face steaming with hot towels and oils, shaving foam made in the traditional way.",
                    Price = 50,
                    Duration = DateTime.Parse("01:15").ToString("HH:mm")
                }
            );

            modelBuilder.Entity<Booking>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    HairdresserId = 1,
                    ServiceId = 4,
                    SpecialOfferId = 2,
                    Date = DateTime.Parse("2023-09-13").ToString("yyyy-MM-dd"),
                    Time = DateTime.Parse("12:45").ToString("HH:mm")
                },
                new Booking
                {
                    Id = 2,
                    HairdresserId = 3,
                    ServiceId = 2,
                    SpecialOfferId = 1,
                    Date = DateTime.Parse("2023-10-01").ToString("yyyy-MM-dd"),
                    Time = DateTime.Parse("10:00").ToString("HH:mm")
                }
            );

            modelBuilder.Entity<SpecialOffer>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<SpecialOffer>().HasData(
                new SpecialOffer
                {
                    Id = 1,
                    Code = RandomHelper.RandomString(10),
                    ServiceId = 2,
                    Percentage = 10,
                    FromDate = DateTime.Parse("2023-09-10").ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Parse("2023-09-17").ToString("yyyy-MM-dd")
                },
                new SpecialOffer
                {
                    Id = 2,
                    Code = RandomHelper.RandomString(10),
                    ServiceId = 1,
                    Percentage = 5,
                    FromDate = DateTime.Parse("2023-12-24").ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Parse("2023-12-26").ToString("yyyy-MM-dd")
                }
            );
        }
    }
}