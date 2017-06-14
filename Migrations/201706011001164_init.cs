namespace DiveCenterRhodes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 128),
                        ServiceId = c.Int(nullable: false),
                        DiveSpotId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        NumOfDives = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShoeNumber = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Paid = c.Boolean(),
                        InstructorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiveSpots", t => t.DiveSpotId, cascadeDelete: true)
                .ForeignKey("dbo.Instructors", t => t.InstructorId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Email)
                .Index(t => t.Email)
                .Index(t => t.ServiceId)
                .Index(t => t.DiveSpotId)
                .Index(t => t.InstructorId);
            
            CreateTable(
                "dbo.DiveSpots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Place = c.String(),
                        Level = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeOfDive = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Level = c.String(),
                        Password = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        PostalCode = c.Int(nullable: false),
                        MedicalHistory = c.String(),
                        Admin = c.Boolean(nullable: false),
                        CouponId = c.Int(),
                    })
                .PrimaryKey(t => t.Email)
                .ForeignKey("dbo.Coupons", t => t.CouponId)
                .Index(t => t.CouponId);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 128),
                        ReviewText = c.String(),
                        Star = c.Int(nullable: false),
                        DiveSpotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiveSpots", t => t.DiveSpotId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Email)
                .Index(t => t.Email)
                .Index(t => t.DiveSpotId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Email", "dbo.Users");
            DropForeignKey("dbo.Reviews", "DiveSpotId", "dbo.DiveSpots");
            DropForeignKey("dbo.Bookings", "Email", "dbo.Users");
            DropForeignKey("dbo.Users", "CouponId", "dbo.Coupons");
            DropForeignKey("dbo.Bookings", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Bookings", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Bookings", "DiveSpotId", "dbo.DiveSpots");
            DropIndex("dbo.Reviews", new[] { "DiveSpotId" });
            DropIndex("dbo.Reviews", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "CouponId" });
            DropIndex("dbo.Bookings", new[] { "InstructorId" });
            DropIndex("dbo.Bookings", new[] { "DiveSpotId" });
            DropIndex("dbo.Bookings", new[] { "ServiceId" });
            DropIndex("dbo.Bookings", new[] { "Email" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Coupons");
            DropTable("dbo.Users");
            DropTable("dbo.Services");
            DropTable("dbo.Instructors");
            DropTable("dbo.DiveSpots");
            DropTable("dbo.Bookings");
        }
    }
}
