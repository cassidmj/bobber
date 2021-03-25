using FluentMigrator;

namespace Bobber.API.Migrations
{
    [Migration(20210310121800)]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            if (Schema.Table("Users").Exists())
            {
                return;
            }

            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("PasswordHash").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("FirstName").AsString().Nullable()
                .WithColumn("LastName").AsString().Nullable();

        }

        public override void Down()
        {
            if (!Schema.Table("Users").Exists())
            {
                return;
            }

            Delete.Table("Users");
        }
    }
}
