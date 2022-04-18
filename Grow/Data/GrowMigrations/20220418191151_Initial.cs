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
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
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
                name: "HealthConcerns",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Concern = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthConcerns", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSources",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSources", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LowIncomeCutOffs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberOfMembers = table.Column<byte>(nullable: false),
                    YearlyIncome = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LowIncomeCutOffs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Method = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GROWAddresses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StreetNumber = table.Column<string>(maxLength: 10, nullable: false),
                    StreetName = table.Column<string>(maxLength: 100, nullable: false),
                    ApartmentNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROWAddresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GROWAddresses_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HouseholdName = table.Column<string>(maxLength: 75, nullable: false),
                    MembershipNumber = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    NumOfMembers = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LICOVerified = table.Column<bool>(nullable: false),
                    LICOVerifiedDate = table.Column<DateTime>(nullable: true),
                    IncomeTotal = table.Column<double>(nullable: false),
                    RenewalDate = table.Column<DateTime>(nullable: false),
                    StreetNumber = table.Column<string>(maxLength: 10, nullable: false),
                    StreetName = table.Column<string>(maxLength: 100, nullable: false),
                    ApartmentNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostalCode = table.Column<string>(nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemNo = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 50, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ItemCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Items_ItemCategories_ItemCategoryID",
                        column: x => x.ItemCategoryID,
                        principalTable: "ItemCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 75, nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    IncomeVerified = table.Column<bool>(nullable: false),
                    IncomeAmount = table.Column<double>(nullable: false),
                    IncomeVerifiedBy = table.Column<string>(nullable: true),
                    DataConsent = table.Column<bool>(nullable: false),
                    EmailConsent = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
                name: "MemberConcerns",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HealthConcernID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberConcerns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberConcerns_HealthConcerns_HealthConcernID",
                        column: x => x.HealthConcernID,
                        principalTable: "HealthConcerns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberConcerns_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberID = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    MimeType = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberDocuments_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberIncomes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IncomeSourceID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    IncomeAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberIncomes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberIncomes_IncomeSources_IncomeSourceID",
                        column: x => x.IncomeSourceID,
                        principalTable: "IncomeSources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberIncomes_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRestrictions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DietaryRestrictionID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRestrictions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberRestrictions_DietaryRestrictions_DietaryRestrictionID",
                        column: x => x.DietaryRestrictionID,
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
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionTotal = table.Column<double>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    HouseholdID = table.Column<int>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    VolunteerID = table.Column<int>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Households_HouseholdID",
                        column: x => x.HouseholdID,
                        principalTable: "Households",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaymentAmount = table.Column<double>(nullable: false),
                    PaymentMethodID = table.Column<int>(nullable: false),
                    TransactionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
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
                    ItemID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitCost = table.Column<double>(nullable: false),
                    ExtendedCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GROWAddresses_CityID",
                table: "GROWAddresses",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Households_CityID",
                table: "Households",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Households_MembershipNumber",
                table: "Households",
                column: "MembershipNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemCategoryID",
                table: "Items",
                column: "ItemCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemName",
                table: "Items",
                column: "ItemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemNo",
                table: "Items",
                column: "ItemNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberConcerns_HealthConcernID",
                table: "MemberConcerns",
                column: "HealthConcernID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberConcerns_MemberID",
                table: "MemberConcerns",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberDocuments_MemberID",
                table: "MemberDocuments",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberIncomes_IncomeSourceID",
                table: "MemberIncomes",
                column: "IncomeSourceID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberIncomes_MemberID",
                table: "MemberIncomes",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRestrictions_DietaryRestrictionID",
                table: "MemberRestrictions",
                column: "DietaryRestrictionID");

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
                name: "IX_Payments_PaymentMethodID",
                table: "Payments",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionID",
                table: "Payments",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_ItemID",
                table: "TransactionDetails",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TransactionID",
                table: "TransactionDetails",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EmployeeID",
                table: "Transactions",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_HouseholdID",
                table: "Transactions",
                column: "HouseholdID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_MemberID",
                table: "Transactions",
                column: "MemberID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GROWAddresses");

            migrationBuilder.DropTable(
                name: "LowIncomeCutOffs");

            migrationBuilder.DropTable(
                name: "MemberConcerns");

            migrationBuilder.DropTable(
                name: "MemberDocuments");

            migrationBuilder.DropTable(
                name: "MemberIncomes");

            migrationBuilder.DropTable(
                name: "MemberRestrictions");

            migrationBuilder.DropTable(
                name: "MembershipChanges");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "HealthConcerns");

            migrationBuilder.DropTable(
                name: "IncomeSources");

            migrationBuilder.DropTable(
                name: "DietaryRestrictions");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
