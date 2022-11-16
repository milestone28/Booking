using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.Dal.Migrations
{
    public partial class roomHotelConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
