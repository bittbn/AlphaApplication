namespace AlphaApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteStatusReservation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "StatusReservationId", "dbo.StatusReservation");
            DropIndex("dbo.Reservation", new[] { "StatusReservationId" });
            AddColumn("dbo.Reservation", "Allow", c => c.Boolean(nullable: false));
            DropColumn("dbo.Reservation", "StatusReservationId");
            DropTable("dbo.StatusReservation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StatusReservation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Reservation", "StatusReservationId", c => c.Int());
            DropColumn("dbo.Reservation", "Allow");
            CreateIndex("dbo.Reservation", "StatusReservationId");
            AddForeignKey("dbo.Reservation", "StatusReservationId", "dbo.StatusReservation", "Id");
        }
    }
}
