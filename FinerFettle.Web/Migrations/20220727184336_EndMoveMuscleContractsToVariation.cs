﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinerFettle.Web.Migrations
{
    public partial class EndMoveMuscleContractsToVariation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MuscleContractions",
                table: "Intensity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MuscleContractions",
                table: "Intensity",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
