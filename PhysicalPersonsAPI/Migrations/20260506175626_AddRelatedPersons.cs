using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhysicalPersonsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelativePersonId1",
                table: "RelatedPersons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPersons_PhysicalPersonId",
                table: "RelatedPersons",
                column: "PhysicalPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPersons_RelativePersonId",
                table: "RelatedPersons",
                column: "RelativePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPersons_RelativePersonId1",
                table: "RelatedPersons",
                column: "RelativePersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_PhysicalPersonId",
                table: "RelatedPersons",
                column: "PhysicalPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId",
                table: "RelatedPersons",
                column: "RelativePersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId1",
                table: "RelatedPersons",
                column: "RelativePersonId1",
                principalTable: "PhysicalPersons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_PhysicalPersonId",
                table: "RelatedPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId",
                table: "RelatedPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_PhysicalPersons_RelativePersonId1",
                table: "RelatedPersons");

            migrationBuilder.DropIndex(
                name: "IX_RelatedPersons_PhysicalPersonId",
                table: "RelatedPersons");

            migrationBuilder.DropIndex(
                name: "IX_RelatedPersons_RelativePersonId",
                table: "RelatedPersons");

            migrationBuilder.DropIndex(
                name: "IX_RelatedPersons_RelativePersonId1",
                table: "RelatedPersons");

            migrationBuilder.DropColumn(
                name: "RelativePersonId1",
                table: "RelatedPersons");
        }
    }
}
