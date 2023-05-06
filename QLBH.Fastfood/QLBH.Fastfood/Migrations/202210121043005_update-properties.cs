namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproperties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DonHang", "Offer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DonHang", "Offer", c => c.Int(nullable: false));
        }
    }
}
