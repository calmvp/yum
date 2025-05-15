using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yum.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderedColumnToCartLineItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ordered",
                table: "CartLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordered",
                table: "CartLineItems");
        }
    }
}
