namespace AlphaApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TimeStart = c.Time(nullable: false, precision: 7),
                        TimeEnd = c.Time(nullable: false, precision: 7),
                        UserId = c.Int(),
                        RoomId = c.Int(),
                        StatusReservationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Room", t => t.RoomId)
                .ForeignKey("dbo.StatusReservation", t => t.StatusReservationId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoomId)
                .Index(t => t.StatusReservationId);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 500),
                        Seats = c.Int(nullable: false),
                        Board = c.Boolean(nullable: false),
                        Projector = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusReservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Login = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Reservation", "UserId", "dbo.User");
            DropForeignKey("dbo.Reservation", "StatusReservationId", "dbo.StatusReservation");
            DropForeignKey("dbo.Reservation", "RoomId", "dbo.Room");
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Reservation", new[] { "StatusReservationId" });
            DropIndex("dbo.Reservation", new[] { "RoomId" });
            DropIndex("dbo.Reservation", new[] { "UserId" });
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.StatusReservation");
            DropTable("dbo.Room");
            DropTable("dbo.Reservation");
        }
    }
}
