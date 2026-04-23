using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGetGamesByPlayerIdStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.GetGamesByPlayerId
                    @PlayerId UNIQUEIDENTIFIER,
                    @Page INT,
                    @PageSize INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @Offset INT = (@Page - 1) * @PageSize;

                    SELECT Id, PlayerChoice, ComputerChoice, Result, CreatedAt, PlayerId
                    FROM Games
                    WHERE PlayerId = @PlayerId
                    ORDER BY CreatedAt DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetGamesByPlayerId;");
        }
    }
}
