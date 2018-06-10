namespace PlumbingRepairService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdornmentElements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        adornmentId = c.Int(nullable: false),
                        elementId = c.Int(nullable: false),
                        count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Adornments", t => t.adornmentId, cascadeDelete: true)
                .ForeignKey("dbo.Elements", t => t.elementId, cascadeDelete: true)
                .Index(t => t.adornmentId)
                .Index(t => t.elementId);
            
            CreateTable(
                "dbo.Adornments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        adornmentName = c.String(nullable: false),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProdOrders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buyerId = c.Int(nullable: false),
                        adornmentId = c.Int(nullable: false),
                        customerId = c.Int(),
                        count = c.Int(nullable: false),
                        sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateCustom = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Adornments", t => t.adornmentId, cascadeDelete: true)
                .ForeignKey("dbo.Buyers", t => t.buyerId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.customerId)
                .Index(t => t.buyerId)
                .Index(t => t.adornmentId)
                .Index(t => t.customerId);
            
            CreateTable(
                "dbo.Buyers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        buyerName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customerName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        elementName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.HangarElements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        hangarId = c.Int(nullable: false),
                        elementId = c.Int(nullable: false),
                        count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Elements", t => t.elementId, cascadeDelete: true)
                .ForeignKey("dbo.Hangars", t => t.hangarId, cascadeDelete: true)
                .Index(t => t.hangarId)
                .Index(t => t.elementId);
            
            CreateTable(
                "dbo.Hangars",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        hangarName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HangarElements", "hangarId", "dbo.Hangars");
            DropForeignKey("dbo.HangarElements", "elementId", "dbo.Elements");
            DropForeignKey("dbo.AdornmentElements", "elementId", "dbo.Elements");
            DropForeignKey("dbo.ProdOrders", "customerId", "dbo.Customers");
            DropForeignKey("dbo.ProdOrders", "buyerId", "dbo.Buyers");
            DropForeignKey("dbo.ProdOrders", "adornmentId", "dbo.Adornments");
            DropForeignKey("dbo.AdornmentElements", "adornmentId", "dbo.Adornments");
            DropIndex("dbo.HangarElements", new[] { "elementId" });
            DropIndex("dbo.HangarElements", new[] { "hangarId" });
            DropIndex("dbo.ProdOrders", new[] { "customerId" });
            DropIndex("dbo.ProdOrders", new[] { "adornmentId" });
            DropIndex("dbo.ProdOrders", new[] { "buyerId" });
            DropIndex("dbo.AdornmentElements", new[] { "elementId" });
            DropIndex("dbo.AdornmentElements", new[] { "adornmentId" });
            DropTable("dbo.Hangars");
            DropTable("dbo.HangarElements");
            DropTable("dbo.Elements");
            DropTable("dbo.Customers");
            DropTable("dbo.Buyers");
            DropTable("dbo.ProdOrders");
            DropTable("dbo.Adornments");
            DropTable("dbo.AdornmentElements");
        }
    }
}
