using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Time2Plan.DAL.Migrations
{
    /// <inheritdoc />
    public partial class relation_between_UP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectEntityId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ProjectEntityId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ActivityID",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ProjectEntityId",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Activities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Activities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProjectId",
                table: "UserProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_UserId",
                table: "Activities");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectID",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityID",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectEntityId",
                table: "Activities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectEntityId",
                table: "Activities",
                column: "ProjectEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectEntityId",
                table: "Activities",
                column: "ProjectEntityId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
