using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.GrowMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DietaryRestrictions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Restriction = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryRestrictions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FinancialStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GenderType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProvinceName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MembershipNumber = table.Column<int>(nullable: false),
                    NumOfMembers = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LICOVerified = table.Column<bool>(nullable: false),
                    LICOVerifiedDate = table.Column<DateTime>(nullable: false),
                    IncomeTotal = table.Column<double>(nullable: false),
                    RenewalDate = table.Column<DateTime>(nullable: false),
                    StreetNumber = table.Column<string>(maxLength: 10, nullable: false),
                    StreetName = table.Column<string>(maxLength: 100, nullable: false),
                    ApartmentNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(nullable: false),
                    ProvinceID = table.Column<int>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Households_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Households_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 75, nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    VolunteerHours = table.Column<double>(nullable: false),
                    StreetNum = table.Column<string>(maxLength: 10, nullable: false),
                    StreetName = table.Column<string>(maxLength: 100, nullable: false),
                    ApartmentNum = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    ProvinceID = table.Column<int>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Volunteers_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Volunteers_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConsentVerified = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 75, nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    IncomeVerified = table.Column<bool>(nullable: false),
                    IncomeAmount = table.Column<double>(nullable: false),
                    DataConsent = table.Column<bool>(nullable: false),
                    HouseholdID = table.Column<int>(nullable: false),
                    GenderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Members_Genders_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Genders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Households_HouseholdID",
                        column: x => x.HouseholdID,
                        principalTable: "Households",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipChanges",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HouseholdID = table.Column<int>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ChangeDescription = table.Column<string>(nullable: true),
                    ChangeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipChanges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MembershipChanges_Households_HouseholdID",
                        column: x => x.HouseholdID,
                        principalTable: "Households",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionTotal = table.Column<double>(nullable: false),
                    HouseholdID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_Households_HouseholdID",
                        column: x => x.HouseholdID,
                        principalTable: "Households",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRestrictions",
                columns: table => new
                {
                    DietaryRestrictionsID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRestrictions", x => new { x.DietaryRestrictionsID, x.MemberID });
                    table.ForeignKey(
                        name: "FK_MemberRestrictions_DietaryRestrictions_DietaryRestrictionsID",
                        column: x => x.DietaryRestrictionsID,
                        principalTable: "DietaryRestrictions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRestrictions_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    MemberID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionID = table.Column<int>(nullable: false),
                    TransactionsID = table.Column<int>(nullable: true),
                    Item = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitCost = table.Column<double>(nullable: false),
                    ExtendedCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Transactions_TransactionsID",
                        column: x => x.TransactionsID,
                        principalTable: "Transactions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    FileContentID = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    MimeType = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.FileContentID);
                    table.ForeignKey(
                        name: "FK_FileContent_UploadedFiles_FileContentID",
                        column: x => x.FileContentID,
                        principalTable: "UploadedFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatuses",
                columns: table => new
                {
                    FinancialStatusID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    MemberDocumentsID = table.Column<int>(nullable: true),
                    IncomeAmount = table.Column<double>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatuses", x => new { x.FinancialStatusID, x.MemberID });
                    table.ForeignKey(
                        name: "FK_MemberStatuses_FinancialStatuses_FinancialStatusID",
                        column: x => x.FinancialStatusID,
                        principalTable: "FinancialStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberStatuses_UploadedFiles_MemberDocumentsID",
                        column: x => x.MemberDocumentsID,
                        principalTable: "UploadedFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberStatuses_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Households_CityID",
                table: "Households",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Households_ProvinceID",
                table: "Households",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRestrictions_MemberID",
                table: "MemberRestrictions",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GenderID",
                table: "Members",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_HouseholdID",
                table: "Members",
                column: "HouseholdID");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipChanges_HouseholdID",
                table: "MembershipChanges",
                column: "HouseholdID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatuses_MemberDocumentsID",
                table: "MemberStatuses",
                column: "MemberDocumentsID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatuses_MemberID",
                table: "MemberStatuses",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TransactionsID",
                table: "TransactionDetails",
                column: "TransactionsID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_HouseholdID",
                table: "Transactions",
                column: "HouseholdID");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_MemberID",
                table: "UploadedFiles",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_CityID",
                table: "Volunteers",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_ProvinceID",
                table: "Volunteers",
                column: "ProvinceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropTable(
                name: "MemberRestrictions");

            migrationBuilder.DropTable(
                name: "MembershipChanges");

            migrationBuilder.DropTable(
                name: "MemberStatuses");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "DietaryRestrictions");

            migrationBuilder.DropTable(
                name: "FinancialStatuses");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
