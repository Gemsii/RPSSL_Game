using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGetPlayerByNameStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.GetPlayerByName
                    @Name NVARCHAR(100)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT Id, Name
                    FROM Players
                    WHERE Name = @Name;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetPlayerByName;");
        }
    }
}
