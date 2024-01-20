using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_layer.Migrations
{
    /// <inheritdoc />
    public partial class NewProretyKilo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Kilo",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Kilo",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kilo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Kilo",
                table: "Cart");
        }
    }
}
