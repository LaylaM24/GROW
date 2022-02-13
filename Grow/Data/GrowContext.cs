﻿using Grow.Models;
using Microsoft.EntityFrameworkCore;

namespace Grow.Data
{
    public class GrowContext : DbContext
    {
        public GrowContext(DbContextOptions<GrowContext> options)
            : base(options)
        {
        }

        public DbSet<Household> Households { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<DietaryRestriction> DietaryRestrictions { get; set; }
        public DbSet<HealthConcern> HealthConcerns { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<MemberIncome> MemberIncomes { get; set; }
        public DbSet<MembershipChange> MembershipChanges { get; set; }
        public DbSet<MemberRestriction> MemberRestrictions { get; set; }
        public DbSet<MemberConcern> MemberConcerns { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<MemberDocument> MemberDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Constraints
            // Household MembershipNumber unique constraint
            modelBuilder.Entity<Household>()
                .HasIndex(h => h.MembershipNumber)
                .IsUnique();
            #endregion

            #region Table Relationships
            // Household - Member Relationship
            modelBuilder.Entity<Household>()
                .HasMany(m => m.Members)
                .WithOne(m => m.Household)
                .HasForeignKey(m => m.HouseholdID)
                .OnDelete(DeleteBehavior.Restrict);

            // Household - HouseholdChange Relationship
            modelBuilder.Entity<Household>()
                .HasMany(m => m.MembershipChanges)
                .WithOne(m => m.Household)
                .HasForeignKey(m => m.HouseholdID);

            // Household - Transaction Relationship
            modelBuilder.Entity<Household>()
                .HasMany(m => m.Transactions)
                .WithOne(m => m.Household)
                .HasForeignKey(m => m.HouseholdID);

            // Household - City Relationship
            modelBuilder.Entity<City>()
                .HasMany(m => m.Households)
                .WithOne(m => m.City)
                .HasForeignKey(m => m.CityID)
                .OnDelete(DeleteBehavior.Restrict);

            // Volunteer - City Relationship
            modelBuilder.Entity<City>()
                .HasMany(m => m.Volunteers)
                .WithOne(m => m.City)
                .HasForeignKey(m => m.CityID)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction - TransactionDetail Relationship
            modelBuilder.Entity<Transaction>()
                .HasMany(m => m.TransactionDetails)
                .WithOne(m => m.Transactions)
                .HasForeignKey(m => m.TransactionID);

            // TransactionDetail - Item Relationship
            modelBuilder.Entity<Item>()
                .HasMany(m => m.TransactionDetails)
                .WithOne(m => m.Item)
                .HasForeignKey(m => m.ItemID);

            // Item - ItemCategory Relationship
            modelBuilder.Entity<ItemCategory>()
                .HasMany(m => m.Items)
                .WithOne(m => m.ItemCategory)
                .HasForeignKey(m => m.ItemCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Member - Gender Relationship
            modelBuilder.Entity<Gender>()
                .HasMany(m => m.Members)
                .WithOne(m => m.Gender)
                .HasForeignKey(m => m.GenderID)
                .OnDelete(DeleteBehavior.Restrict);

            // Member - MemberDocument Relationship
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MemberDocuments)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberID);

            // Member - MemberIncome Relationship
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MemberIncomes)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberID);

            // Member - MemberRestriction Relationship
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MemberRestrictions)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberID);

            // Member - MemberConcern Relationship
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MemberConcerns)
                .WithOne(m => m.Member)
                .HasForeignKey(m => m.MemberID);

            // MemberIncome - IncomeSource Relationship
            modelBuilder.Entity<IncomeSource>()
                .HasMany(m => m.MemberIncomes)
                .WithOne(m => m.IncomeSource)
                .HasForeignKey(m => m.IncomeSourceID)
                .OnDelete(DeleteBehavior.Restrict);

            // MemberRestriction - DietaryRestriction Relationship
            modelBuilder.Entity<DietaryRestriction>()
                .HasMany(m => m.MemberRestrictions)
                .WithOne(m => m.DietaryRestriction)
                .HasForeignKey(m => m.DietaryRestrictionID)
                .OnDelete(DeleteBehavior.Restrict);

            // MemberConcern - HealthConcern Relationship
            modelBuilder.Entity<HealthConcern>()
                .HasMany(m => m.MemberConcerns)
                .WithOne(m => m.HealthConcern)
                .HasForeignKey(m => m.HealthConcernID)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
