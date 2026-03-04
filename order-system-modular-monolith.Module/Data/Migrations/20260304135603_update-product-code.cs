using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace order_system_modular_monolith.Product.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateproductcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "products",
                table: "products",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "code",
                schema: "products",
                table: "products");
        }
    }
}
