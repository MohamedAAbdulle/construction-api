using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionApi.Migrations
{
    public partial class tablenamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubContractsDb",
                table: "SubContractsDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorDb",
                table: "ContractorDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractItemDb",
                table: "ContractItemDb");

            migrationBuilder.RenameTable(
                name: "SubContractsDb",
                newName: "SubContract");

            migrationBuilder.RenameTable(
                name: "ContractorDb",
                newName: "Contractor");

            migrationBuilder.RenameTable(
                name: "ContractItemDb",
                newName: "ContractItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubContract",
                table: "SubContract",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contractor",
                table: "Contractor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractItem",
                table: "ContractItem",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubContract",
                table: "SubContract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contractor",
                table: "Contractor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractItem",
                table: "ContractItem");

            migrationBuilder.RenameTable(
                name: "SubContract",
                newName: "SubContractsDb");

            migrationBuilder.RenameTable(
                name: "Contractor",
                newName: "ContractorDb");

            migrationBuilder.RenameTable(
                name: "ContractItem",
                newName: "ContractItemDb");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubContractsDb",
                table: "SubContractsDb",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorDb",
                table: "ContractorDb",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractItemDb",
                table: "ContractItemDb",
                column: "Id");
        }
    }
}
