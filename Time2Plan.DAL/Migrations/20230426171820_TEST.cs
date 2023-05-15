using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Time2Plan.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TEST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowNickName",
                table: "Users",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShowNickName",
                table: "Users");
        }
    }
}
