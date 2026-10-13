using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMINEmployeeMaster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    CompanyId = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmployeeNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmpName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmpNameSecondLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AliasSecondLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NFCUId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EmpCategoryId = table.Column<int>(type: "int", nullable: false),
                    EmpCategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmpTypeId = table.Column<int>(type: "int", nullable: false),
                    EmpTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmpJoinedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreviousEmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ResignDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMINEmployeeMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADMINEmployeeMaster_TenantId_CompanyId_SiteId_EmployeeNo",
                table: "ADMINEmployeeMaster",
                columns: new[] { "TenantId", "CompanyId", "SiteId", "EmployeeNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMINEmployeeMaster");
        }
    }
}
