using Grow.Models;
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
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<FinancialStatus> FinancialStatuses { get; set; }
        public DbSet<DietaryRestrictions> DietaryRestrictions { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MemberRestrictions> MemberRestrictions { get; set; }
        public DbSet<MembershipChanges> MembershipChanges { get; set; }
        public DbSet<TransactionDetails> TransactionDetails { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<MemberStatus> MemberStatuses { get; set; }
        public DbSet<MemberDocument> MemberDocuments { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Preventing cascade deletes
            //deleting a Membership where Members are assigned

            modelBuilder.Entity<Household>()
                .HasMany<Member>(ms => ms.Members)
                .WithOne(m => m.Household)
                .HasForeignKey(m => m.HouseholdID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberRestrictions>()
                .HasKey(mr => new { mr.DietaryRestrictionsID, mr.MemberID });

            modelBuilder.Entity<MemberRestrictions>()
                .HasOne(mr => mr.DietaryRestrictions)
                .WithMany(dr => dr.MemberRestrictions)
                .HasForeignKey(mr => mr.DietaryRestrictionsID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberStatus>()
                .HasKey(mr => new { mr.FinancialStatusID, mr.MemberID });

            modelBuilder.Entity<MemberStatus>()
                .HasOne(mr => mr.FinancialStatus)
                .WithMany(fs => fs.MemberStatus)
                .HasForeignKey(mr => mr.FinancialStatusID)
                .OnDelete(DeleteBehavior.Restrict);

            //unique restrictions if needed
        }
    }
}
