namespace TaobaoMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class haha : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderHeaders", "TotalPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderHeaders", "TotalPrice", c => c.Int(nullable: false));
        }
    }
}
