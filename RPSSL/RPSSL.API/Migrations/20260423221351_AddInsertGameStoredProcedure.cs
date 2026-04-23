using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInsertGameStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.InsertGame
                    @Id UNIQUEIDENTIFIER,
                    @PlayerChoice INT,
                    @ComputerChoice INT,
                    @Result INT,
                    @CreatedAt DATETIME2,
                    @PlayerId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;
                    INSERT INTO Games (Id, PlayerChoice, ComputerChoice, Result, CreatedAt, PlayerId)
                    VALUES (@Id, @PlayerChoice, @ComputerChoice, @Result, @CreatedAt, @PlayerId);
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.InsertGame;");
        }
    }
}
