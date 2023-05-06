namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SanPham", "TenSP", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SanPham", "TenSP", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
