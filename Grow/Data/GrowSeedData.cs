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
                        },
                        new City
                        {
                            CityName = "Fort Erie"
                        },
                        new City
                        {
                            CityName = "Grimsby"
                        },
                        new City
                        {
                            CityName = "Lincoln"
                        },
                        new City
                        {
                            CityName = "West Lincoln"
                        },
                        new City
                        {
                            CityName = "Pelham"
                        },
                        new City
                        {
                            CityName = "Wainfleet"
                        },
                        new City
                        {
                            CityName = "Niagara-on-the-Lake"
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
                        },
                        new Gender
                        {
                            GenderType = "Non-Binary"
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
                            CategoryName = "Specials"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Pantry"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Dairy/Eggs/Bread"
                        },                        
                        new ItemCategory
                        {
                            CategoryName = "Freezer"
                        },
                        new ItemCategory
                        {
                            CategoryName = "Produce"
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

                #region GROW Addresses
                if (!context.GROWAddresses.Any())
                {
                    var city = context.Cities.Where(x => x.CityName == "Niagara Falls").FirstOrDefault();

                    context.GROWAddresses.AddRange(
                        new GROWAddress
                        {
                           StreetNumber = "4377",
                           StreetName = "Fourth Ave.",
                           PostalCode = "L2E 4N1",
                           CityID = city.ID,
                           City = city
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
                            ItemNo = 149,
                            ItemName = "Anise / Fennel",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 102,
                            ItemName = "Avocado (large)*",
                            Price = 1.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 103,
                            ItemName = "Avocado (small)*",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 105,
                            ItemName = "Blueberries / Blackberries",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 106,
                            ItemName = "Broccoli",
                            Price = 2.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 147,
                            ItemName = "Brussel Sprouts",
                            Price = 1.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 127,
                            ItemName = "Cabbage*",
                            Price = 2.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 107,
                            ItemName = "Cantaloupe",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 108,
                            ItemName = "Carrots",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 109,
                            ItemName = "Cauliflower*",
                            Price = 2.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 110,
                            ItemName = "Celery",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 111,
                            ItemName = "Clementine",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 112,
                            ItemName = "Corn",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 113,
                            ItemName = "Cucumber",
                            Price = 1.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 114,
                            ItemName = "Cucumber Mini",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 115,
                            ItemName = "Eggplant",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 116,
                            ItemName = "Garlic*",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 117,
                            ItemName = "Grapes*",
                            Price = 1.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 118,
                            ItemName = "Green Onions",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 119,
                            ItemName = "Kale",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 120,
                            ItemName = "Kiwi",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 121,
                            ItemName = "Lemon*",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 122,
                            ItemName = "Lettuce Romaine Hearts",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 123,
                            ItemName = "Limes",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 124,
                            ItemName = "Mango",
                            Price = 1.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 125,
                            ItemName = "Micro Greens",
                            Price = 0.25,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 126,
                            ItemName = "Mushrooms*",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 128,
                            ItemName = "Onion",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 129,
                            ItemName = "Oranges",
                            Price = 0.20,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 130,
                            ItemName = "Peaches / Nectarines",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 131,
                            ItemName = "Pear",
                            Price = 0.10,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 132,
                            ItemName = "Peppers",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 133,
                            ItemName = "Peppers Hot (3)",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 134,
                            ItemName = "Peppers Mini ",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 135,
                            ItemName = "Plums",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 136,
                            ItemName = "Potatoes",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },                        
                        new Item
                        {
                            ItemNo = 137,
                            ItemName = "Potatoes Baby Basket",
                            Price = .50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 138,
                            ItemName = "Potatoes Sweet (Yam)",
                            Price = 0.75,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 139,
                            ItemName = "Raspberries",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 140,
                            ItemName = "Squash",
                            Price = 0.00,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 141,
                            ItemName = "Strawberries",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 142,
                            ItemName = "Swiss Chard",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 143,
                            ItemName = "Tomato Cherry / Grape Basket",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 144,
                            ItemName = "Tomatoes",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 145,
                            ItemName = "Watermelon",
                            Price = 2.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 146,
                            ItemName = "Zucchini",
                            Price = 0.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 148,
                            ItemName = "Shallots",
                            Price = 0.05,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 150,
                            ItemName = "Bag of Potatoes",
                            Price = 1.50,
                            ItemCategoryID = 1
                        },
                        new Item
                        {
                            ItemNo = 201,
                            ItemName = "Chicken Legs (2)",
                            Price = 1.00,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 202,
                            ItemName = "Chicken Drumsticks (4lbs)",
                            Price = 3.00,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 203,
                            ItemName = "Chicken Thighs (4lbs)",
                            Price = 3.00,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 204,
                            ItemName = "Chicken Wings (2lbs)",
                            Price = 2.00,
                            ItemCategoryID = 2
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
                            ItemNo = 206,
                            ItemName = "Veggie Burger (2 pack)",
                            Price = 2.00,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 207,
                            ItemName = "Fish (Haddock / Basa)",
                            Price = 2.75,
                            ItemCategoryID = 2
                        },
                        new Item
                        {
                            ItemNo = 446,
                            ItemName = "Coffee",
                            Price = 4.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 401,
                            ItemName = "Apple Sauce",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 402,
                            ItemName = "Baking Powder",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 403,
                            ItemName = "Bars Cereal, Protein, Cookie",
                            Price = 0.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 404,
                            ItemName = "BBQ Sauce",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 405,
                            ItemName = "Bleach",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 406,
                            ItemName = "Broth",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 407,
                            ItemName = "Canned Beans, Veggies, and Fruit",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 408,
                            ItemName = "Canola Oil",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 409,
                            ItemName = "Cereal All Other",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 410,
                            ItemName = "Cereal Rice Krispies",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 445,
                            ItemName = "Coconut Milk",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 411,
                            ItemName = "Crackers",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 412,
                            ItemName = "Dried Legumes / Beans",
                            Price = 1.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 413,
                            ItemName = "Flour",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 414,
                            ItemName = "Garden Cocktail",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 415,
                            ItemName = "Granola Bars (6 pack)",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 449,
                            ItemName = "Gummies",
                            Price = 0.10,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 416,
                            ItemName = "Jam",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 417,
                            ItemName = "Kraft Dinner",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 418,
                            ItemName = "Laundry Soap (large)",
                            Price = 6.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 419,
                            ItemName = "Laundry Soap (small)",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 420,
                            ItemName = "Miracle Whip",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 421,
                            ItemName = "Nuts",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 422,
                            ItemName = "Oats",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 423,
                            ItemName = "Olive Oil",
                            Price = 6.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 424,
                            ItemName = "Passata",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 425,
                            ItemName = "Pasta",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 426,
                            ItemName = "Pasta Sauce",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 427,
                            ItemName = "Peanut Butter",
                            Price = 2.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 428,
                            ItemName = "Polenta",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 429,
                            ItemName = "Rice",
                            Price = 1.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 447,
                            ItemName = "Protein Drink",
                            Price = 0.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 450,
                            ItemName = "Raisins",
                            Price = 4.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 430,
                            ItemName = "Salad Dressing",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 448,
                            ItemName = "Salsa",
                            Price = 1.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 431,
                            ItemName = "Soap",
                            Price = 0.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 432,
                            ItemName = "Soup (small)",
                            Price = 0.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 433,
                            ItemName = "Spices",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 434,
                            ItemName = "Sugar White and Brown",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 435,
                            ItemName = "Tea",
                            Price = 2.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 436,
                            ItemName = "Tea Green Tea",
                            Price = 4.50,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 437,
                            ItemName = "Tea Orange Pekoe",
                            Price = 3.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 438,
                            ItemName = "Tea Red Rose",
                            Price = 5.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 439,
                            ItemName = "Toilet Paper",
                            Price = 5.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 440,
                            ItemName = "Tomato Paste",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 441,
                            ItemName = "Tooth Paste / Brush / Floss",
                            Price = 0.75,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 442,
                            ItemName = "Tuna",
                            Price = 1.00,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 443,
                            ItemName = "Wild Rice Blend",
                            Price = 0.25,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 444,
                            ItemName = "Yeast",
                            Price = 0.25,
                            ItemCategoryID = 3
                        },
                        new Item
                        {
                            ItemNo = 301,
                            ItemName = "Almond Milk (2L)",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 302,
                            ItemName = "Bread Costco",
                            Price = 0.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 303,
                            ItemName = "Butter",
                            Price = 1.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 304,
                            ItemName = "Cheese (large)",
                            Price = 3.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 305,
                            ItemName = "Cream Cheese",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 306,
                            ItemName = "Eggs (12)",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 307,
                            ItemName = "Goat Milk (1L)",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 308,
                            ItemName = "Hummus",
                            Price = 2.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 309,
                            ItemName = "Hummis Mini",
                            Price = 0.25,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 310,
                            ItemName = "Margerine",
                            Price = 1.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 311,
                            ItemName = "Milk (1L)",
                            Price = 1.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 312,
                            ItemName = "Milk (4L)",
                            Price = 3.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 313,
                            ItemName = "Oat Milk (1L)",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 314,
                            ItemName = "Orange Juice",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 315,
                            ItemName = "Pizza Dough",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 316,
                            ItemName = "Sour Crème",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 317,
                            ItemName = "Soy Milk (1L)",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 318,
                            ItemName = "Tofu",
                            Price = 2.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 319,
                            ItemName = "Yogurt (4 pack)",
                            Price = 1.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 320,
                            ItemName = "Yogurt Greek",
                            Price = 3.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 321,
                            ItemName = "Yogurt Tub",
                            Price = 2.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 322,
                            ItemName = "Yogurt (6 pack)",
                            Price = 1.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 324,
                            ItemName = "Bread Commisso's",
                            Price = 1.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 323,
                            ItemName = "Sliced Cheese",
                            Price = 2.50,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 325,
                            ItemName = "Eggs (18)",
                            Price = 3.00,
                            ItemCategoryID = 4
                        },
                        new Item
                        {
                            ItemNo = 601,
                            ItemName = "Cat Food (wet)",
                            Price = 0.50,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 602,
                            ItemName = "Sweets (Costco)",
                            Price = 0.00,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 603,
                            ItemName = "Drinks",
                            Price = 0.50,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 604,
                            ItemName = "GROW Soup",
                            Price = 2.50,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 605,
                            ItemName = "Deoderant",
                            Price = 1.00,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 606,
                            ItemName = "Orzo",
                            Price = 0.75,
                            ItemCategoryID = 5
                        },
                        new Item
                        {
                            ItemNo = 607,
                            ItemName = "Ramen / Rice Krispies",
                            Price = 0.25,
                            ItemCategoryID = 5
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
                            EmailConsent = false,
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
                            EmailConsent = true,
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
                            EmailConsent = true,
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
                            EmailConsent = true,
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
                            EmailConsent = true,
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
                            ChangeDescription = "Household Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 2,
                            ChangeType = "Create",
                            ChangeDescription = "Household Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 3,
                            ChangeType = "Create",
                            ChangeDescription = "Household Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 4,
                            ChangeType = "Create",
                            ChangeDescription = "Household Created",
                            ChangeDate = DateTime.Now
                        },
                        new MembershipChange
                        {
                            HouseholdID = 5,
                            ChangeType = "Create",
                            ChangeDescription = "Household Created",
                            ChangeDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();
                }
                #endregion

                //#region Employee
                //// Look for any Employees.  Seed ones to match the seeded Identity accounts.
                //if (!context.Employees.Any())
                //{
                //    context.Employees.AddRange(
                //     new Employee
                //     {
                //         FirstName = "Brooke",
                //         LastName = "Smith",
                //         Email = "admin1@outlook.com"
                //     },
                //     new Employee
                //     {
                //         FirstName = "Connor",
                //         LastName = "Mack",
                //         Email = "super1@outlook.com"
                //     },
                //     new Employee
                //     {
                //         FirstName = "Layla",
                //         LastName = "Roe",
                //         Email = "volunteer1@outlook.com"
                //     },
                //     new Employee
                //     {
                //         FirstName = "Matthew",
                //         LastName = "Stevens",
                //         Email = "user1@outlook.com"
                //     });

                //    context.SaveChanges();
                //}
                //#endregion
            }
        }
    }
}