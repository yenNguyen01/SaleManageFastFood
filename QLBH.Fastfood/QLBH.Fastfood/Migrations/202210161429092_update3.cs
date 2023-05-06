namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaiKhoan", "Email", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaiKhoan", "Email", c => c.String(nullable: false, unicode: false));
        }
    }
}
