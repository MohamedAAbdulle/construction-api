using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionApi.Migrations
{
    public partial class addcustomerIdsubcontracttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SubContractsDb",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SubContractsDb");
        }
    }
}
