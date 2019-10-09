namespace TestTask.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relatives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateBirth = c.DateTime(nullable: false),
                        Gender = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relatives", "ParentId", "dbo.Users");
            DropIndex("dbo.Relatives", new[] { "ParentId" });
            DropTable("dbo.Users");
            DropTable("dbo.Relatives");
        }
    }
}
