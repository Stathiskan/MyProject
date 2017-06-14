namespace DiveCenterRhodes.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DiveCenterRhodes.Models;


    internal sealed class Configuration : DbMigrationsConfiguration<DiveCenterRhodes.Models.MyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DiveCenterRhodes.Models.MyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //Add our services
            context.Services.AddOrUpdate(x => x.TypeOfDive,
                new Service { TypeOfDive = "Dive", Price = 50M },
                new Service { TypeOfDive = "School", Price = 100M });

            context.SaveChanges();

            //Add Coupons
            context.Coupons.AddOrUpdate(x => x.Description,
                new Coupon { Type = "Bronze", Description = "After 3 dives 5% discount for the next dive!" },
                new Coupon { Type = "Silver", Description = "After 8 dives 20% discount for the next dive!" },
                new Coupon { Type = "Gold", Description = "After 11 dives 50% discount for the next dive!" });
            context.SaveChanges();

            //Add our dive spots
            context.DiveSpots.AddOrUpdate(x => x.Place,
                new DiveSpot { Place = "St. Paul's Bay", Level = "Amatuer" },
                new DiveSpot { Place = "Great cave \"Cleobulus Tomb\"", Level = "Pro" },
                new DiveSpot { Place = "Plimmiri - \"Giannoula K.\" shipwreck", Level = "Amatuer" },
                new DiveSpot { Place = "Pentanissos islet", Level = "Amatuer" });

            context.SaveChanges();
            //Add instructors
            context.Instructors.AddOrUpdate(x => x.Name,
                new Instructor { Name = "Michalis", Level = "Master" },
                new Instructor { Name = "Maria", Level = "Pro" },
                new Instructor { Name = "Stathis", Level = "Master" },
                new Instructor { Name = "Nikos", Level = "Master" },
                new Instructor { Name = "Alekos", Level = "Pro" },
                new Instructor { Name = "Katerina", Level = "Master" },
                new Instructor { Name = "Panos", Level = "Pro" },
                new Instructor { Name = "Danai", Level = "Master" },
                new Instructor { Name = "Evaggelia", Level = "Pro" });

            context.SaveChanges();

            //Add admin
            context.Users.AddOrUpdate(x => x.Email,
                new User { Email = "michaelAdmin@gmail.com", FirstName = "Michalis", LastName = "Archontis", DateOfBirth = new DateTime(1985, 10, 22), Level = "Master", Password = "12345", Country = "Greece", Address = "Alexandras 21", PostalCode = 12345, PhoneNumber = "6980797310", MedicalHistory = "None", Admin = true });

            context.SaveChanges();
        }
    }
}
