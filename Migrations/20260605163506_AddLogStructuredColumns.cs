using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiraisTask.Migrations
{
    /// <inheritdoc />
    public partial class AddLogStructuredColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "Logs",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrpcMethod",
                table: "Logs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogEvent",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceContext",
                table: "Logs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CorrelationId",
                table: "Logs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GrpcMethod",
                table: "Logs",
                column: "GrpcMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_CorrelationId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GrpcMethod",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "GrpcMethod",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "LogEvent",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "SourceContext",
                table: "Logs");
        }
    }
}
