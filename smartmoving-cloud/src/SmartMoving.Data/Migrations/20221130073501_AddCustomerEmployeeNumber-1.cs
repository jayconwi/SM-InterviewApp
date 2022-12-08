using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMoving.Data.Migrations
{
    public partial class AddCustomerEmployeeNumber1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                schema: "Core",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "0" 
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                schema: "Core",
                table: "Customers"
            );
        }
    }
}
