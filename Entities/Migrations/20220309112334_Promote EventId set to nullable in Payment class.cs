using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class PromoteEventIdsettonullableinPaymentclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PromoteEvents_PromoteEventId",
                table: "Payments");

            migrationBuilder.AlterColumn<Guid>(
                name: "PromoteEventId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PromoteEvents_PromoteEventId",
                table: "Payments",
                column: "PromoteEventId",
                principalTable: "PromoteEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PromoteEvents_PromoteEventId",
                table: "Payments");

            migrationBuilder.AlterColumn<Guid>(
                name: "PromoteEventId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PromoteEvents_PromoteEventId",
                table: "Payments",
                column: "PromoteEventId",
                principalTable: "PromoteEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
