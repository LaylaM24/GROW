﻿// <auto-generated />
using System;
using Grow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Grow.Data.GrowMigrations
{
    [DbContext(typeof(GrowContext))]
    [Migration("20220418191151_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22");

            modelBuilder.Entity("Grow.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Grow.Models.DietaryRestriction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Restriction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("DietaryRestrictions");
                });

            modelBuilder.Entity("Grow.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Grow.Models.GROWAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<int>("CityID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("GROWAddresses");
                });

            modelBuilder.Entity("Grow.Models.Gender", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GenderType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("Grow.Models.HealthConcern", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Concern")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("HealthConcerns");
                });

            modelBuilder.Entity("Grow.Models.Household", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<int>("CityID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("HouseholdName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(75);

                    b.Property<double>("IncomeTotal")
                        .HasColumnType("REAL");

                    b.Property<bool>("LICOVerified")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LICOVerifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MembershipNumber")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("NumOfMembers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RenewalDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.HasIndex("MembershipNumber")
                        .IsUnique();

                    b.ToTable("Households");
                });

            modelBuilder.Entity("Grow.Models.IncomeSource", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("IncomeSources");
                });

            modelBuilder.Entity("Grow.Models.Item", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemCategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<int>("ItemNo")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.HasIndex("ItemCategoryID");

                    b.HasIndex("ItemName")
                        .IsUnique();

                    b.HasIndex("ItemNo")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Grow.Models.ItemCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("Grow.Models.LowIncomeCutOff", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("NumberOfMembers")
                        .HasColumnType("INTEGER");

                    b.Property<double>("YearlyIncome")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.ToTable("LowIncomeCutOffs");
                });

            modelBuilder.Entity("Grow.Models.Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<bool>("DataConsent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<bool>("EmailConsent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(75);

                    b.Property<int>("GenderID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HouseholdID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("IncomeAmount")
                        .HasColumnType("REAL");

                    b.Property<bool>("IncomeVerified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IncomeVerifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(75);

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("GenderID");

                    b.HasIndex("HouseholdID");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Grow.Models.MemberConcern", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HealthConcernID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.HasIndex("HealthConcernID");

                    b.HasIndex("MemberID");

                    b.ToTable("MemberConcerns");
                });

            modelBuilder.Entity("Grow.Models.MemberDocument", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MimeType")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("MemberID");

                    b.ToTable("MemberDocuments");
                });

            modelBuilder.Entity("Grow.Models.MemberIncome", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("IncomeAmount")
                        .HasColumnType("REAL");

                    b.Property<int>("IncomeSourceID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("IncomeSourceID");

                    b.HasIndex("MemberID");

                    b.ToTable("MemberIncomes");
                });

            modelBuilder.Entity("Grow.Models.MemberRestriction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DietaryRestrictionID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.HasIndex("DietaryRestrictionID");

                    b.HasIndex("MemberID");

                    b.ToTable("MemberRestrictions");
                });

            modelBuilder.Entity("Grow.Models.MembershipChange", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChangeDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HouseholdID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("HouseholdID");

                    b.ToTable("MembershipChanges");
                });

            modelBuilder.Entity("Grow.Models.Payment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("PaymentAmount")
                        .HasColumnType("REAL");

                    b.Property<int>("PaymentMethodID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransactionID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("PaymentMethodID");

                    b.HasIndex("TransactionID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Grow.Models.PaymentMethod", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("Grow.Models.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HouseholdID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Paid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("TransactionTotal")
                        .HasColumnType("REAL");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("HouseholdID");

                    b.HasIndex("MemberID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Grow.Models.TransactionDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ExtendedCost")
                        .HasColumnType("REAL");

                    b.Property<int>("ItemID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransactionID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitCost")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.HasIndex("ItemID");

                    b.HasIndex("TransactionID");

                    b.ToTable("TransactionDetails");
                });

            modelBuilder.Entity("Grow.Models.GROWAddress", b =>
                {
                    b.HasOne("Grow.Models.City", "City")
                        .WithMany("GROWAddresses")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.Household", b =>
                {
                    b.HasOne("Grow.Models.City", "City")
                        .WithMany("Households")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.Item", b =>
                {
                    b.HasOne("Grow.Models.ItemCategory", "ItemCategory")
                        .WithMany("Items")
                        .HasForeignKey("ItemCategoryID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.Member", b =>
                {
                    b.HasOne("Grow.Models.Gender", "Gender")
                        .WithMany("Members")
                        .HasForeignKey("GenderID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Grow.Models.Household", "Household")
                        .WithMany("Members")
                        .HasForeignKey("HouseholdID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.MemberConcern", b =>
                {
                    b.HasOne("Grow.Models.HealthConcern", "HealthConcern")
                        .WithMany("MemberConcerns")
                        .HasForeignKey("HealthConcernID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Grow.Models.Member", "Member")
                        .WithMany("MemberConcerns")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.MemberDocument", b =>
                {
                    b.HasOne("Grow.Models.Member", "Member")
                        .WithMany("MemberDocuments")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.MemberIncome", b =>
                {
                    b.HasOne("Grow.Models.IncomeSource", "IncomeSource")
                        .WithMany("MemberIncomes")
                        .HasForeignKey("IncomeSourceID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Grow.Models.Member", "Member")
                        .WithMany("MemberIncomes")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.MemberRestriction", b =>
                {
                    b.HasOne("Grow.Models.DietaryRestriction", "DietaryRestriction")
                        .WithMany("MemberRestrictions")
                        .HasForeignKey("DietaryRestrictionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Grow.Models.Member", "Member")
                        .WithMany("MemberRestrictions")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.MembershipChange", b =>
                {
                    b.HasOne("Grow.Models.Household", "Household")
                        .WithMany("MembershipChanges")
                        .HasForeignKey("HouseholdID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.Payment", b =>
                {
                    b.HasOne("Grow.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentMethodID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Grow.Models.Transaction", "Transaction")
                        .WithMany("Payments")
                        .HasForeignKey("TransactionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.Transaction", b =>
                {
                    b.HasOne("Grow.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID");

                    b.HasOne("Grow.Models.Household", "Household")
                        .WithMany("Transactions")
                        .HasForeignKey("HouseholdID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Grow.Models.Member", "Member")
                        .WithMany("Transactions")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grow.Models.TransactionDetail", b =>
                {
                    b.HasOne("Grow.Models.Item", "Item")
                        .WithMany("TransactionDetails")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Grow.Models.Transaction", "Transactions")
                        .WithMany("TransactionDetails")
                        .HasForeignKey("TransactionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
