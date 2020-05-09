using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication3.Migrations
{
    public partial class seedEmployeestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Department", "Name", "email" },
                values: new object[] { 1, 1, "Indra", "indra@gmail.com" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Department", "Name", "email" },
                values: new object[] { 2, 2, "Indra", "Abhi@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
