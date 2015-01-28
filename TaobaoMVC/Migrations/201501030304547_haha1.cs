namespace TaobaoMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class haha1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "OrderHeader_Id", "dbo.OrderHeaders");
            DropIndex("dbo.OrderDetails", new[] { "OrderHeader_Id" });
            AlterColumn("dbo.OrderDetails", "OrderHeader_Id", c => c.Int());
            AddForeignKey("dbo.OrderDetails", "OrderHeader_Id", "dbo.OrderHeaders", "Id");
            CreateIndex("dbo.OrderDetails", "OrderHeader_Id");
            DropColumn("dbo.OrderDetails", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Price", c => c.Int(nullable: false));
            DropIndex("dbo.OrderDetails", new[] { "OrderHeader_Id" });
            DropForeignKey("dbo.OrderDetails", "OrderHeader_Id", "dbo.OrderHeaders");
            AlterColumn("dbo.OrderDetails", "OrderHeader_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderDetails", "OrderHeader_Id");
            AddForeignKey("dbo.OrderDetails", "OrderHeader_Id", "dbo.OrderHeaders", "Id", cascadeDelete: true);
        }
    }
}
