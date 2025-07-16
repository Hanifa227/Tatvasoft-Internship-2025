using Microsoft.EntityFrameworkCore;
using Mission.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.context
{
    public class MissionDbContext : DbContext
    {
        public MissionDbContext(DbContextOptions<MissionDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<MissionTheme> MissionTheme { get; set; }
        public DbSet<MissionSkill> MissionSkill { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Missions> Missions { get; set; }
        public DbSet<MissionApplication> MissionApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegancyTimestampBehavior", true);
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FirstName = "Tatva",
                LastName = "Admin",
                EmailAddress = "admin@tatvasoft.com",
                UserType = "admin",
                Password = "Tatva@123",
                PhoneNumber = "9876543210",
                CreatedDate = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
           

            );

            modelBuilder.Entity<Country>().HasData(new Country()
            {
                Id = 1,
                CountryName = "India",
            });

            modelBuilder.Entity<City>().HasData(new City()
            {
                Id = 1,
                CityName = "Ahmedabad",
                CountryId = 1
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
