using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceNegotiator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_tables_negotiations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negotiation_Products_ProductId",
                table: "Negotiation");

            migrationBuilder.DropForeignKey(
                name: "FK_NegotiationAttempt_Negotiation_NegotiationId",
                table: "NegotiationAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NegotiationAttempt",
                table: "NegotiationAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Negotiation",
                table: "Negotiation");

            migrationBuilder.RenameTable(
                name: "NegotiationAttempt",
                newName: "NegotiationAttempts");

            migrationBuilder.RenameTable(
                name: "Negotiation",
                newName: "Negotiations");

            migrationBuilder.RenameIndex(
                name: "IX_NegotiationAttempt_NegotiationId",
                table: "NegotiationAttempts",
                newName: "IX_NegotiationAttempts_NegotiationId");

            migrationBuilder.RenameIndex(
                name: "IX_Negotiation_ProductId",
                table: "Negotiations",
                newName: "IX_Negotiations_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NegotiationAttempts",
                table: "NegotiationAttempts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Negotiations",
                table: "Negotiations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NegotiationAttempts_Negotiations_NegotiationId",
                table: "NegotiationAttempts",
                column: "NegotiationId",
                principalTable: "Negotiations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Negotiations_Products_ProductId",
                table: "Negotiations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NegotiationAttempts_Negotiations_NegotiationId",
                table: "NegotiationAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Negotiations_Products_ProductId",
                table: "Negotiations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Negotiations",
                table: "Negotiations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NegotiationAttempts",
                table: "NegotiationAttempts");

            migrationBuilder.RenameTable(
                name: "Negotiations",
                newName: "Negotiation");

            migrationBuilder.RenameTable(
                name: "NegotiationAttempts",
                newName: "NegotiationAttempt");

            migrationBuilder.RenameIndex(
                name: "IX_Negotiations_ProductId",
                table: "Negotiation",
                newName: "IX_Negotiation_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_NegotiationAttempts_NegotiationId",
                table: "NegotiationAttempt",
                newName: "IX_NegotiationAttempt_NegotiationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Negotiation",
                table: "Negotiation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NegotiationAttempt",
                table: "NegotiationAttempt",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Negotiation_Products_ProductId",
                table: "Negotiation",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NegotiationAttempt_Negotiation_NegotiationId",
                table: "NegotiationAttempt",
                column: "NegotiationId",
                principalTable: "Negotiation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
