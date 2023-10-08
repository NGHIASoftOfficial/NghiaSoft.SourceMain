using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NghiaSoft.FileServerAPI.Migrations
{
    public partial class Update_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Actor",
                table: "AppFileActionDetails",
                newName: "ActorUserName");

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthenticateRequired",
                table: "AppFileInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwnedVisibleOnly",
                table: "AppFileInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ActorId",
                table: "AppFileActionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuthenticateRequired",
                table: "AppFileInfos");

            migrationBuilder.DropColumn(
                name: "IsOwnedVisibleOnly",
                table: "AppFileInfos");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "AppFileActionDetails");

            migrationBuilder.RenameColumn(
                name: "ActorUserName",
                table: "AppFileActionDetails",
                newName: "Actor");
        }
    }
}
