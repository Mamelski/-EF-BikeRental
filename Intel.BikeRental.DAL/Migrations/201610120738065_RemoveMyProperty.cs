namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMyProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("rentals.Users", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("rentals.Users", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
