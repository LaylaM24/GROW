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
                #region Provinces
                if (!context.Provinces.Any())
                {
                    context.Provinces.AddRange(
                        new Province
                        {
                            ProvinceName = "Alberta"
                        },
                        new Province
                        {
                            ProvinceName = "British Columbia"
                        },
                        new Province
                        {
                            ProvinceName = "Manitoba"
                        },
                        new Province
                        {
                            ProvinceName = "New Brunswick"
                        },
                        new Province
                        {
                            ProvinceName = "Newfoundland and Labrador"
                        },
                        new Province
                        {
                            ProvinceName = "Northwest Territories"
                        },
                        new Province
                        {
                            ProvinceName = "Nova Scotia"
                        },
                        new Province
                        {
                            ProvinceName = "Nunavut"
                        },
                        new Province
                        {
                            ProvinceName = "Ontario"
                        },
                        new Province
                        {
                            ProvinceName = "Prince Edward Island"
                        },
                        new Province
                        {
                            ProvinceName = "Quebec"
                        },
                        new Province
                        {
                            ProvinceName = "Saskatchewan"
                        },
                        new Province
                        {
                            ProvinceName = "Yukon"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

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
                            CityName = "Welland"
                        },
                        new City
                        {
                            CityName = "Thunder Bay"
                        },
                        new City
                        {
                            CityName = "Vancouver"
                        },
                        new City
                        {
                            CityName = "Niagara Falls"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                #region Dietary Restrictions
                if (!context.DietaryRestrictions.Any())
                {
                    context.DietaryRestrictions.AddRange(
                        new DietaryRestrictions
                        {
                            Restriction = "Other"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Caffine Intolerance"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Food Allergies"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Digestive Disorders"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Osteoperosis"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Heart Disease"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Cancer"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Gluten Intolerance"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Lactose Intolerance"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Obesity"
                        },
                        new DietaryRestrictions
                        {
                            Restriction = "Diabetes"
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
                            GenderType = "Prefer not to say"
                        },
                        new Gender
                        {
                            GenderType = "Other"
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

                #region Financial Statuses
                if (!context.FinancialStatuses.Any())
                {
                    context.FinancialStatuses.AddRange(
                        new FinancialStatus
                        {
                            Status = "ODSP"
                        },
                        new FinancialStatus
                        {
                            Status = "Ontario Works"
                        },
                        new FinancialStatus
                        {
                            Status = "CPP-Disability"
                        },
                        new FinancialStatus
                        {
                            Status = "EI"
                        },
                        new FinancialStatus
                        {
                            Status = "GAINS"
                        },
                        new FinancialStatus
                        {
                            Status = "Post-Secondary Student"
                        },
                        new FinancialStatus
                        {
                            Status = "Other"
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                // Everything below to be removed at later date 
                #region Households
                if (!context.Households.Any())
                {
                    context.Households.AddRange(
                        new Household
                        {
                            MembershipNumber = 1,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 1000,
                            RenewalDate = DateTime.Now,
                            StreetNumber = "123",
                            StreetName = "Main St.",
                            PostalCode = "l2n2y3",
                            ProvinceID = 9,
                            CityID = 1
                        },
                        new Household
                        {
                            MembershipNumber = 2,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = false,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 5000,
                            RenewalDate = DateTime.Now,
                            StreetNumber = "700",
                            StreetName = "Geneva Ave.",
                            PostalCode = "k2n3h3",
                            ProvinceID = 2,
                            CityID = 2
                        },
                        new Household
                        {
                            MembershipNumber = 3,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 20000,
                            RenewalDate = DateTime.Now,
                            StreetNumber = "412",
                            StreetName = "Lakeshore Dr.",
                            PostalCode = "p2f8k2",
                            ProvinceID = 9,
                            CityID = 3
                        },
                        new Household
                        {
                            MembershipNumber = 4,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = true,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 15000,
                            RenewalDate = DateTime.Now,
                            StreetNumber = "743",
                            StreetName = "Queen St.",
                            PostalCode = "2n42i4",
                            ProvinceID = 9,
                            CityID = 4,
                        },
                        new Household
                        {
                            MembershipNumber = 5,
                            NumOfMembers = 1,
                            CreatedDate = DateTime.Now,
                            LICOVerified = false,
                            LICOVerifiedDate = DateTime.Now,
                            IncomeTotal = 10000,
                            RenewalDate = DateTime.Now,
                            StreetNumber = "321",
                            StreetName = "West Ave.",
                            PostalCode = "l4n1y3",
                            ProvinceID = 9,
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
                            ConsentVerified = true,
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
                            ConsentVerified = true,
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
                            ConsentVerified = true,
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
                            ConsentVerified = true,
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
                            ConsentVerified = true,
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

                #region Member Status
                if (!context.MemberStatuses.Any())
                {
                    context.MemberStatuses.AddRange(
                        new MemberStatus
                        {
                            FinancialStatusID = 7,
                            MemberID = 4,
                            IncomeAmount = 15000
                        },
                        new MemberStatus
                        {
                            FinancialStatusID = 7,
                            MemberID = 3,
                            IncomeAmount = 20000
                        },
                        new MemberStatus
                        {
                            FinancialStatusID = 2,
                            MemberID = 3,
                            IncomeAmount = 0
                        },
                        new MemberStatus
                        {
                            FinancialStatusID = 3,
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
                            ProvinceID = 1,
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
                            ProvinceID = 2,
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
                            ProvinceID = 3,
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
                            ProvinceID = 4,
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
                            ProvinceID = 5,
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
                        new MemberRestrictions
                        {
                            MemberID = 1,
                            DietaryRestrictionsID = 8
                        },
                        new MemberRestrictions
                        {
                            MemberID = 2,
                            DietaryRestrictionsID = 6
                        },
                        new MemberRestrictions
                        {
                            MemberID = 3,
                            DietaryRestrictionsID = 5
                        },
                        new MemberRestrictions
                        {
                            MemberID = 4,
                            DietaryRestrictionsID = 9
                        }
                    );
                    context.SaveChanges();
                }
                #endregion
            }
        }
    }
}