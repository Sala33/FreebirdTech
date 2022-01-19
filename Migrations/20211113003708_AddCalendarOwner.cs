using Microsoft.EntityFrameworkCore.Migrations;

namespace FreebirdTech.Migrations
{
    public partial class AddCalendarOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "Message",
                type: "character varying(170)",
                maxLength: 170,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "CalendarEvent",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicoId",
                table: "CalendarEvent",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvent_ArtistId",
                table: "CalendarEvent",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvent_ServicoId",
                table: "CalendarEvent",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvent_Artists_ArtistId",
                table: "CalendarEvent",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvent_Servicos_ServicoId",
                table: "CalendarEvent",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvent_Artists_ArtistId",
                table: "CalendarEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvent_Servicos_ServicoId",
                table: "CalendarEvent");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvent_ArtistId",
                table: "CalendarEvent");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvent_ServicoId",
                table: "CalendarEvent");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "CalendarEvent");

            migrationBuilder.DropColumn(
                name: "ServicoId",
                table: "CalendarEvent");

            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "Message",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(170)",
                oldMaxLength: 170);
        }
    }
}
