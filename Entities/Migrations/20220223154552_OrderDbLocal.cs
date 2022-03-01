﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class OrderDbLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusOrder",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusOrder",
                table: "Orders");
        }
    }
}
