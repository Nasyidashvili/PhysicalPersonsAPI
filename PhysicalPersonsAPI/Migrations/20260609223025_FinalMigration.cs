using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhysicalPersonsAPI.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId1",
                table: "RelatedPersons");

            migrationBuilder.DropIndex(
                name: "IX_RelatedPersons_RelativePersonId1",
                table: "RelatedPersons");

            migrationBuilder.DropColumn(
                name: "RelativePersonId1",
                table: "RelatedPersons");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_PhysicalPersonId",
                table: "PhoneNumbers",
                column: "PhysicalPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers",
                column: "PhysicalPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers");

            migrationBuilder.DropIndex(
                name: "IX_PhoneNumbers_PhysicalPersonId",
                table: "PhoneNumbers");

            migrationBuilder.AddColumn<int>(
                name: "RelativePersonId1",
                table: "RelatedPersons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPersons_RelativePersonId1",
                table: "RelatedPersons",
                column: "RelativePersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId1",
                table: "RelatedPersons",
                column: "RelativePersonId1",
                principalTable: "PhysicalPersons",
                principalColumn: "Id");
        }
    }
}
