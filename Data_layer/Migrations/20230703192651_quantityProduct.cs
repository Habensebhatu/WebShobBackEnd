using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_layer.Migrations
{
    /// <inheritdoc />
    public partial class quantityProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "quantityProduct",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantityProduct",
                table: "Category");
        }
    }
}
