namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaiKhoan", "MatKhau", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaiKhoan", "MatKhau", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
