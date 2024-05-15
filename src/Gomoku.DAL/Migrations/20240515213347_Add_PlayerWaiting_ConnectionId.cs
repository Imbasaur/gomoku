using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gomoku.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_PlayerWaiting_ConnectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "WaitingList",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "WaitingList");
        }
    }
}
