using Microsoft.EntityFrameworkCore.Migrations;

namespace Allison.Model.Migrations
{
    public partial class DBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageBubble",
                columns: table => new
                {
                    MessageBubbleId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    To = table.Column<bool>(nullable: false),
                    Yo = table.Column<bool>(nullable: false),
                    Select = table.Column<bool>(nullable: false),
                    Youtube = table.Column<bool>(nullable: false),
                    News = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    LiveTime = table.Column<string>(nullable: true),
                    VideoTitle = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true),
                    VideoImage = table.Column<string>(nullable: true),
                    NewsTitle = table.Column<string>(nullable: true),
                    NewsTime = table.Column<string>(nullable: true),
                    NewsUrl = table.Column<string>(nullable: true),
                    NewsImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageBubble", x => x.MessageBubbleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageBubble");
        }
    }
}
