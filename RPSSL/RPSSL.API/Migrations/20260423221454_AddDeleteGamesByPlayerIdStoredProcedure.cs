using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteGamesByPlayerIdStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.DeleteGamesByPlayerId
                    @PlayerId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DELETE FROM Games WHERE PlayerId = @PlayerId;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.DeleteGamesByPlayerId;");
        }
    }
}
