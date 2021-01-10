namespace ProductManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongDescriptionNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "LongDescription", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "LongDescription", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
