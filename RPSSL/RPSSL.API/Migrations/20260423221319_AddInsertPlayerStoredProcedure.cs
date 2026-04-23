using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInsertPlayerStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.InsertPlayer
                    @Id UNIQUEIDENTIFIER,
                    @Name NVARCHAR(100)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    INSERT INTO Players (Id, Name)
                    VALUES (@Id, @Name);
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.InsertPlayer;");
        }
    }
}
