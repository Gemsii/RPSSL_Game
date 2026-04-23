using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGetPlayerByIdStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.GetPlayerById
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT Id, Name
                    FROM Players
                    WHERE Id = @Id;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetPlayerById;");
        }
    }
}
