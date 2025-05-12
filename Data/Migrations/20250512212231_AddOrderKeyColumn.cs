using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yum.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderKeyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderKey",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderKey",
                table: "Orders");
        }
    }
}
