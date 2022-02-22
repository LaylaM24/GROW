using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Grow.Models;
using System.Linq;
using System.Collections.Generic;

namespace Grow.Data
{
    public static class GrowSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GrowContext(
                serviceProvider.GetRequiredService<DbContextOptions<GrowContext>>()))
            {
                // Look-up tables
                #region Cities
                if (!context.Cities.Any())
                {
                    context.Cities.AddRange(
                        new City
                        {
                            CityName = "St. Catharines"
                        },
                        new City
                        {
                            CityName = "Niagara Falls"
                        },
                        new City
                        {
                            CityName = "Thorold"
                        },
                        new City
                        {
                            CityName = "Port Colborne"
                        },
                        new City
                        {
                            CityName = "Welland"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Dietary Restrictions
                if (!context.DietaryRestrictions.Any())
                {
                    context.DietaryRestrictions.AddRange(
                        new DietaryRestriction
                        {
                            Restriction = "Other"
                        },
                        new DietaryRestriction
                        {
                            Restriction = "Caffine Intolerance"
                        },
                        new DietaryRestriction
                        {
                            Restriction = "Food Allergy"
                        },
                        new DietaryRestriction
                        {
                            Restriction = "Gluten Intolerance"
                        },
                        new DietaryRestriction
                        {
                            Restriction = "Lactose Intolerance"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Health Concerns
                if (!context.HealthConcerns.Any())
                {
                    context.HealthConcerns.AddRange(
                        new HealthConcern
                        {
                            Concern = "Other"
                        },
                        new HealthConcern
                        {
                            Concern = "Osteoperosis"
                        },
                        new HealthConcern
                        {
                            Concern = "Heart Disease"
                        },
                        new HealthConcern
                        {
                            Concern = "Cancer"
                        },
                        new HealthConcern
                        {
                            Concern = "Obesity"
                        },
                        new HealthConcern
                        {
                            Concern = "Diabetes"
                        },
                        new HealthConcern
                        {
                            Concern = "Digestive Disorder"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Genders
                if (!context.Genders.Any())
                {
                    context.Genders.AddRange(
                        new Gender
                        {
                            GenderType = "Other"
                        },
                        new Gender
                        {
                            GenderType = "Prefer not to say"
                        },
                        new Gender
                        {
                            GenderType = "Female"
                        },
                        new Gender
                        {
                            GenderType = "Male"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Income Sources
                if (!context.IncomeSources.Any())
                {
                    context.IncomeSources.AddRange(
                        new IncomeSource
                        {
                            Source = "Other"
                        },
                        new IncomeSource
                        {
                            Source = "ODSP"
                        },
                        new IncomeSource
                        {
                            Source = "Ontario Works"
                        },
                        new IncomeSource
                        {
                            Source = "CPP-Disability"
                        },
                        new IncomeSource
                        {
                            Source = "EI"
                        },
                        new IncomeSource
                        {
                            Source = "GAINS"
                        },
                        new IncomeSource
                        {
                            Source = "Post-Secondary Student"
                        },
                        new IncomeSource
                        {
                            Source = "WSIB"
                        },
                        new IncomeSource
                        {
                            Source = "Employed"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Item Categories
                if (!context.ItemCategories.Any())
                {
                    context.ItemCategories.AddRange(
                        new ItemCategory
                        {
                            CategoryName = "Produce"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Freezer"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Pantry"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Dairy/Eggs/Bread"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Low Income Cut Offs
                if (!context.LowIncomeCutOffs.Any())
                {
                    context.LowIncomeCutOffs.AddRange(
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 1,
                            YearlyIncome = 22186,
                            MonthlyIncome = 1849
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 2,
                            YearlyIncome = 27619,
                            MonthlyIncome = 2302
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 3,
                            YearlyIncome = 33953,
                            MonthlyIncome = 2830
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 4,
                            YearlyIncome = 41225,
                            MonthlyIncome = 3435
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 5,
                            YearlyIncome = 46757,
                            MonthlyIncome = 3896
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 6,
                            YearlyIncome = 52734,
                            MonthlyIncome = 4395
                        },
                        new LowIncomeCutOff
                        {
                            NumberOfMembers = 7,
                            YearlyIncome = 58712,
                            MonthlyIncome = 4893
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                // Test data
                #region Items
                if (!context.Items.Any())
                {
                    context.Items.AddRange(
                        new Item
                        {
                            ItemNo = 101,
                            ItemName = "Apples",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 104,
                            ItemName = "Bananas",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 205,
                            ItemName = "Ground Beef",
                            Price = 2.75,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 445,
                            ItemName = "Coffee",
                            Price = 4.00,
                            ItemCategoryID = 3
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Households
                if (!context.Households.Any())
                {
                    context.Households.AddRange(
                        new Household
                        {
                            MembershipNumber = 1,
                            HouseholdName = "Bonds",
                            Active = true,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 1000,
                            RenewalDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day),
                            StreetNumber = "123",
                            StreetName = "Main St.",
                            PostalCode = "L2N2Y3",
                            CityID = 1
                        },
                        new Household
                        {
                            MembershipNumber = 2,
                            HouseholdName = "Walker",
                            Active = true,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = false,
                            IncomeTotal = 5000,
                            RenewalDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day),
                            StreetNumber = "700",
                            StreetName = "Geneva Ave.",
                            PostalCode = "K2N3H3",
                            CityID = 2
                        },
                        new Household
                        {
                            MembershipNumber = 3,
                            HouseholdName = "Cob",
                            Active = false,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 20000,
                            RenewalDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day),
                            StreetNumber = "412",
                            StreetName = "Lakeshore Dr.",
                            PostalCode = "P2F8K2",
                            CityID = 3
                        },
                        new Household
                        {
                            MembershipNumber = 4,
                            HouseholdName = "Ortiz",
                            Active = true,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 15000,
                            RenewalDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day),
                            StreetNumber = "743",
                            StreetName = "Queen St.",
                            PostalCode = "2N42I4",
                            CityID = 4,
                        },
                        new Household
                        {
                            MembershipNumber = 5,
                            HouseholdName = "Clemens",
                            Active = true,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = false,
                            IncomeTotal = 10000,
                            RenewalDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day),
                            StreetNumber = "321",
                            StreetName = "West Ave.",
                            PostalCode = "L4N1Y3",
                            CityID = 5
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Members
                if (!context.Members.Any())
                {
                    context.Members.AddRange(
                        new Member
                        {
                            HouseholdID = 1,
                            FirstName = "Barry",
                            LastName = "Bonds",
                            DOB = Convert.ToDateTime("1966-05-04"),
                            Phone = "9053332343",
                            Email = "BarryBonds@hotmail.com",
                            IncomeVerified = true,
                            IncomeAmount = 10000,
                            DataConsent = true,
                            GenderID = 1
                        },
                        new Member
                        {
                            HouseholdID = 2,
                            FirstName = "Larry",
                            LastName = "Walker",
                            DOB = Convert.ToDateTime("1939-09-18"),
                            Phone = "2894321444",
                            Email = "LarryWalker@hotmail.com",
                            IncomeVerified = false,
                            IncomeAmount = 15000,
                            DataConsent = true,
                            GenderID = 1
                        },
                        new Member
                        {
                            HouseholdID = 3,
                            FirstName = "Ty",
                            LastName = "Cob",
                            DOB = Convert.ToDateTime("1949-12-20"),
                            Phone = "9051249292",
                            Email = "TyCob@hotmail.com",
                            IncomeVerified = true,
                            IncomeAmount = 20000,
                            DataConsent = false,
                            GenderID = 1
                        },
                        new Member
                        {
                            HouseholdID = 4,
                            FirstName = "David",
                            LastName = "Ortiz",
                            DOB = Convert.ToDateTime("1960-10-27"),
                            Phone = "2895128571",
                            Email = "DavidOrtiz@hotmail.com",
                            IncomeVerified = true,
                            IncomeAmount = 5000,
                            DataConsent = true,
                            GenderID = 1
                        },
                        new Member
                        {
                            HouseholdID = 5,
                            FirstName = "Roger",
                            LastName = "Clemens",
                            DOB = Convert.ToDateTime("1991-07-15"),
                            Phone = "9051241111",
                            Email = "RogerClemens@hotmail.com",
                            IncomeVerified = false,
                            IncomeAmount = 1000,
                            DataConsent = true,
                            GenderID = 1
                        });
                    context.SaveChanges();
                }
                #endregion

                #region Member Income
                if (!context.MemberIncomes.Any())
                {
                    context.MemberIncomes.AddRange(
                        new MemberIncome
                        {
                            IncomeSourceID = 7,
                            MemberID = 4,
                            IncomeAmount = 15000
                        },
                        new MemberIncome
                        {
                            IncomeSourceID = 7,
                            MemberID = 3,
                            IncomeAmount = 20000
                        },
                        new MemberIncome
                        {
                            IncomeSourceID = 2,
                            MemberID = 3,
                            IncomeAmount = 0
                        },
                        new MemberIncome
                        {
                            IncomeSourceID = 3,
                            MemberID = 2,
                            IncomeAmount = 5000
                        });
                    context.SaveChanges();
                }
                #endregion

                #region Volunteers
                if (!context.Volunteers.Any())
                {
                    context.Volunteers.AddRange(
                        new Volunteer
                        {
                            FirstName = "Matt",
                            LastName = "Bruce",
                            Phone = "2895018561",
                            Email = "matthewbruce@hotmail.com",
                            StartDate = DateTime.Now,
                            VolunteerHours = 0,
                            StreetName = "Cresent Ave.",
                            StreetNum = "72",
                            PostalCode = "p2k2p2",
                            CityID = 1
                        },
                        new Volunteer
                        {
                            FirstName = "Mike",
                            LastName = "Bruce",
                            Phone = "9051245524",
                            Email = "mikebruce@hotmail.com",
                            StartDate = DateTime.Now,
                            VolunteerHours = 20,
                            StreetName = "Lakeport Bvld.",
                            StreetNum = "123",
                            PostalCode = "n1n2j3",
                            CityID = 2
                        },
                        new Volunteer
                        {
                            FirstName = "Olivia",
                            LastName = "Port",
                            Phone = "9125231682",
                            Email = "oliviaport@hotmail.com",
                            StartDate = DateTime.Now,
                            VolunteerHours = 1140,
                            StreetName = "Queenstone Stn.",
                            StreetNum = "42",
                            PostalCode = "L4n 3kp",
                            CityID = 3
                        },
                        new Volunteer
                        {
                            FirstName = "Rebecca",
                            LastName = "White",
                            Phone = "9052127566",
                            Email = "beccawhite@hotmail.com",
                            StartDate = DateTime.Now,
                            VolunteerHours = 0,
                            StreetName = "Niagara St.",
                            StreetNum = "625",
                            PostalCode = "t9n3h4",
                            CityID = 4
                        },
                        new Volunteer
                        {
                            FirstName = "Ryan",
                            LastName = "West",
                            Phone = "2895237323",
                            Email = "Ryanwest@hotmail.com",
                            StartDate = DateTime.Now,
                            VolunteerHours = 800,
                            StreetName = "Terrance Lane",
                            StreetNum = "36",
                            PostalCode = "d8d2n4",
                            CityID = 5
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Member Restrictions
                if (!context.MemberRestrictions.Any())
                {
                    context.MemberRestrictions.AddRange(
                        new MemberRestriction
                        {
                            MemberID = 1,
                            DietaryRestrictionID = 1
                        },
                        new MemberRestriction
                        {
                            MemberID = 2,
                            DietaryRestrictionID = 4
                        },
                        new MemberRestriction
                        {
                            MemberID = 3,
                            DietaryRestrictionID = 5
                        },
                        new MemberRestriction
                        {
                            MemberID = 4,
                            DietaryRestrictionID = 3
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Member Concerns
                if (!context.MemberConcerns.Any())
                {
                    context.MemberConcerns.AddRange(
                        new MemberConcern
                        {
                            MemberID = 1,
                            HealthConcernID = 1
                        },
                        new MemberConcern
                        {
                            MemberID = 2,
                            HealthConcernID = 2
                        },
                        new MemberConcern
                        {
                            MemberID = 3,
                            HealthConcernID = 3
                        },
                        new MemberConcern
                        {
                            MemberID = 4,
                            HealthConcernID = 4
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Membership Changes
                if (!context.MembershipChanges.Any())
                {
                    context.MembershipChanges.AddRange(
                        new MembershipChange
                        {
                            HouseholdID = 1,
                            ChangeType = "Create",
                            ChangeDescription = "Membership Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 2,
                            ChangeType = "Create",
                            ChangeDescription = "Membership Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 3,
                            ChangeType = "Create",
                            ChangeDescription = "Membership Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 4,
                            ChangeType = "Create",
                            ChangeDescription = "Membership Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 5,
                            ChangeType = "Create",
                            ChangeDescription = "Membership Created",
                            ChangeDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Transaction
                if (!context.Transactions.Any())
                {
                    context.Transactions.AddRange(
                        new Transaction
                        {
                            HouseholdID = 1,
                            TransactionDate = DateTime.Now,
                            TransactionTotal = 6.00,
                            VolunteerID = 1
                        },
                        new Transaction
                        {
                            HouseholdID = 2,
                            TransactionDate = DateTime.Now,
                            TransactionTotal = 2.75,
                            VolunteerID = 2
                        },
                        new Transaction
                        {
                            HouseholdID = 3,
                            TransactionDate = DateTime.Now,
                            TransactionTotal = 4.50,
                            VolunteerID = 3
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Transaction Details
                if (!context.TransactionDetails.Any())
                {
                    context.TransactionDetails.AddRange(
                        new TransactionDetail
                        {
                            TransactionID = 1,
                            ItemID = 1,
                            Quantity = 10,
                            UnitCost = 0.10,
                            ExtendedCost = 1.00
                        },
                        new TransactionDetail
                        {
                            TransactionID = 1,
                            ItemID = 2,
                            Quantity = 10,
                            UnitCost = 0.10,
                            ExtendedCost = 1.00
                        },
                        new TransactionDetail
                        {
                            TransactionID = 1,
                            ItemID = 4,
                            Quantity = 1,
                            UnitCost = 4.00,
                            ExtendedCost = 4.00
                        },
                        new TransactionDetail
                        {
                            TransactionID = 2,
                            ItemID = 3,
                            Quantity = 1,
                            UnitCost = 2.75,
                            ExtendedCost = 2.75
                        },
                        new TransactionDetail
                        {
                            TransactionID = 3,
                            ItemID = 4,
                            Quantity = 1,
                            UnitCost = 4.00,
                            ExtendedCost = 4.00
                        },
                        new TransactionDetail
                        {
                            TransactionID = 3,
                            ItemID = 2,
                            Quantity = 5,
                            UnitCost = 0.10,
                            ExtendedCost = 0.50
                        }
                    );
                    context.SaveChanges();
                }
                #endregion
            }
        }
    }
}