using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Migrations
{
    public partial class Procedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                    @"
                        EXEC ('CREATE PROCEDURE GetOrders
	                                @Month int = NULL,
	                                @Year int = NULL,
	                                @OrderStatus int = NULL,
	                                @ProductId int = NULL
                                AS
                                BEGIN
	                                SET NOCOUNT ON;
	
	                                 SELECT
		                                Id,
		                                Status,
		                                CreatedDate,
		                                UpdatedDate,
		                                ProductId
                                    FROM [Orders]
                                    WHERE
                                        (@Month = 0 OR MONTH(CreatedDate) = @Month)
                                    AND (@Year = 0 OR YEAR(CreatedDate) = @Year)
	                                AND (@OrderStatus = 0 OR Status = @OrderStatus)
	                                AND (@ProductId = 0 OR ProductId = @ProductId)
                                    ORDER BY CreatedDate DESC
                                END
                                ')");


            migrationBuilder.Sql(
                    @"
                        EXEC ('CREATE PROCEDURE DeleteOrders
	                                @Month int = NULL,
	                                @Year int = NULL,
	                                @OrderStatus int = NULL,
	                                @ProductId int = NULL
                                AS
                                BEGIN
	                                SET NOCOUNT ON;
	
	                                DELETE FROM [Orders]
                                    WHERE
                                        (@Month = 0 OR MONTH(CreatedDate) = @Month)
                                    AND (@Year = 0 OR YEAR(CreatedDate) = @Year)
	                                AND (@OrderStatus = 0 OR Status = @OrderStatus)
	                                AND (@ProductId = 0 OR ProductId = @ProductId)
                                END
                                ')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
