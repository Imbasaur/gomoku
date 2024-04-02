using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gomoku.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Game_concurrency_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Games",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Games");
        }
    }
}
