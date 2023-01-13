using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionApi.Migrations
{
    public partial class toolsidsrequired2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InUseTool",
                table: "InUseTool");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InUseTool",
                table: "InUseTool",
                columns: new[] { "ToolId", "WorkerId", "DateAssigned" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InUseTool",
                table: "InUseTool");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InUseTool",
                table: "InUseTool",
                columns: new[] { "ToolId", "WorkerId" });
        }
    }
}
