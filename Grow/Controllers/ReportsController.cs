using Grow.Data;
using Grow.Models;
using Grow.Utilities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Controllers
{
    public class ReportsController : Controller
    {
        private readonly GrowContext _context;

        public ReportsController(GrowContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            CookieHelper.CookieSet(HttpContext, ControllerName() + "URL", "", -1);

            return View();
        }

        public ActionResult ViewReport(string id = null)
        {
            // Redirect to correct action based on report selection
            if (id == "MembershipRenewal")
            {
                return RedirectToAction("MembershipRenewal");
            }
            else if (id == "HouseholdLocations")
            {
                return RedirectToAction("HouseholdLocations");
            }
            else if (id == "HouseholdIncome")
            {
                return RedirectToAction("HouseholdIncome");
            }
            else if (id == "HouseholdVisits")
            {
                return RedirectToAction("HouseholdVisits");
            }
            else if (id == "MemberAges")
            {
                return RedirectToAction("MemberAges");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> MembershipRenewal(int? page, int? pageSizeID, string actionButton, bool cbInactive,
            string sortDirection = "asc", string sortField = "Membership No.", string date = "")
        {
            ViewData["Date"] = null;
            ViewData["ExclInactive"] = cbInactive;

            string[] sortOptions = new[] { "Membership No.", "No. Members", "Created Date", "LICO Verified", "LICO Verified Date", "Income Total", "Renewal Date" };

            var households = _context.Households.AsQueryable();

            if(date != "" && date != null)
            {
                try
                {
                    DateTime d = Convert.ToDateTime(date);
                    households = households.Where(x => x.RenewalDate <= d);
                    ViewData["Date"] = d.ToShortDateString();
                }
                catch { }
            }

            if(cbInactive == true)
            {
                households = households.Where(x => x.RenewalDate >= DateTime.Today);
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (sortField == "No. Members")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.NumOfMembers);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.NumOfMembers);
                }
            }
            else if (sortField == "Created Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.CreatedDate);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.CreatedDate);
                }
            }
            else if (sortField == "LICO Verified")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.LICOVerified);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerified);
                }
            }
            else if (sortField == "LICO Verified Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.LICOVerifiedDate);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerifiedDate);
                }
            }
            else if (sortField == "Income Total")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.IncomeTotal);
                }
            }
            else if (sortField == "Renewal Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.RenewalDate);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.RenewalDate);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.MembershipNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.MembershipNumber);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> HouseholdLocations(int? page, int? pageSizeID,
            string actionButton, bool cbInactive, string sortDirection = "asc", string sortField = "Membership No.", string City = "-1")
        {
            ViewData["SelectedCity"] = City;
            ViewData["ExclInactive"] = cbInactive;

            string[] sortOptions = new[] { "Membership No.", "No. Members", "Total Income", "Street No.", "Street Name", "Apartment No.", "Postal Code" };

            var households = _context.Households
                .Include(x => x.City)
                .AsQueryable();

            if(City != "-1")
            {
                households = households.Where(x => x.CityID == Convert.ToInt32(City));
            }

            if (cbInactive == true)
            {
                households = households.Where(x => x.RenewalDate >= DateTime.Today);
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (sortField == "No. Members")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.NumOfMembers);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.NumOfMembers);
                }
            }
            else if (sortField == "Total Income")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.IncomeTotal);
                }
            }
            else if (sortField == "Street No.")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.StreetNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.StreetNumber);
                }
            }
            else if (sortField == "Street Name")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.StreetName);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.StreetName);
                }
            }
            else if (sortField == "Apartment No.")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.ApartmentNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.ApartmentNumber);
                }
            }
            else if (sortField == "Postal Code")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.PostalCode);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.PostalCode);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.MembershipNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.MembershipNumber);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            // Get Cities SelectList
            var cities = _context.Cities.OrderBy(x => x.CityName);
            var citiesList = new List<SelectListItem>();

            citiesList.Add(new SelectListItem("All Cities", "-1", City == "-1" ? true : false));

            foreach(var city in cities)
            {
                citiesList.Add(new SelectListItem(city.CityName, city.ID.ToString(), City == city.ID.ToString() ? true : false));
            }

            ViewData["City"] = citiesList.AsEnumerable();

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> HouseholdIncome(int? page, int? pageSizeID,
            string actionButton, string LowRange, string HighRange, bool cbInactive, string sortDirection = "asc", string sortField = "Membership No.")
        {
            ViewData["ExclInactive"] = cbInactive;

            string[] sortOptions = new[] { "Membership No.", "No. Members", "Total Income", "LICO Verified", "LICO Verified Date" };

            var households = _context.Households.AsQueryable();

            if (LowRange != null && HighRange != null)
            {
                households = households.Where(h => h.IncomeTotal >= Convert.ToDouble(LowRange) && h.IncomeTotal <= Convert.ToDouble(HighRange));
            }

            if (cbInactive == true)
            {
                households = households.Where(x => x.RenewalDate >= DateTime.Today);
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (sortField == "No. Members")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.NumOfMembers);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.NumOfMembers);
                }
            }
            else if (sortField == "Total Income")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.IncomeTotal);
                }
            }
            else if (sortField == "LICO Verified")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.LICOVerified);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerified);
                }
            }
            else if (sortField == "LICO Verified Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.LICOVerifiedDate);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerifiedDate);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.MembershipNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.MembershipNumber);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["lowRange"] = LowRange;
            ViewData["highRange"] = HighRange;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> HouseholdVisits(int? page, int? pageSizeID,
            string actionButton, bool cbInactive, string sortDirection = "asc", string sortField = "Membership No.")
        {
            ViewData["ExclInactive"] = cbInactive;

            string[] sortOptions = new[] { "Membership No.", "No. Members", "Total Income", "City", "No. Visits" };

            var households = _context.Households
                .Include(x => x.City)
                .Include(x => x.Transactions)
                .AsQueryable();

            if (cbInactive == true)
            {
                households = households.Where(x => x.RenewalDate >= DateTime.Today);
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (sortField == "No. Members")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.NumOfMembers);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.NumOfMembers);
                }
            }
            else if (sortField == "Total Income")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.IncomeTotal);
                }
            }
            else if (sortField == "City")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.City.CityName);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.City.CityName);
                }
            }
            else if (sortField == "No. Visits")
            {
                var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day);
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.Transactions.Where(x => x.TransactionDate >= date).Count());
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.Transactions.Where(x => x.TransactionDate >= date).Count());
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderBy(m => m.MembershipNumber);
                }
                else
                {
                    households = households
                        .OrderByDescending(m => m.MembershipNumber);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> MemberAges(int? page, int? pageSizeID,
            string actionButton, string LowRange, string HighRange, bool cbInactive, string sortDirection = "asc", string sortField = "Name")
        {
            ViewData["ExclInactive"] = cbInactive;

            string[] sortOptions = new[] { "Name", "Age", "Gender", "Income", "Income Verified" };

            var members = _context.Members
                .Include(x => x.Household)
                .Include(x => x.Gender)
                .AsQueryable();

            if (LowRange != null && HighRange != null)
            {
                members = members.Where(m => DateTime.Today.Year - m.DOB.Year - ((DateTime.Today.Month < m.DOB.Month || (DateTime.Today.Month == m.DOB.Month && DateTime.Today.Day < m.DOB.Day) ? 1 : 0)) >= Convert.ToInt32(LowRange) &&
                DateTime.Today.Year - m.DOB.Year - ((DateTime.Today.Month < m.DOB.Month || (DateTime.Today.Month == m.DOB.Month && DateTime.Today.Day < m.DOB.Day) ? 1 : 0)) <= Convert.ToInt32(HighRange));
            }

            if (cbInactive == true)
            {
                members = members.Where(x => x.Household.RenewalDate >= DateTime.Today);
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (sortField == "Age")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.DOB);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.DOB);
                }
            }
            else if (sortField == "Gender")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.Gender.GenderType);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.Gender.GenderType);
                }
            }
            else if (sortField == "Income")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.IncomeAmount);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.IncomeAmount);
                }
            }
            else if (sortField == "Income Verified")
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.IncomeVerified);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.IncomeVerified);
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    members = members
                        .OrderBy(m => m.LastName).ThenBy(m => m.FirstName);
                }
                else
                {
                    members = members
                        .OrderByDescending(m => m.LastName).ThenByDescending(m => m.FirstName);
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["lowRange"] = LowRange;
            ViewData["highRange"] = HighRange;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public IActionResult DownloadMembershipRenewal(bool exclInactive, string date = "")
        {
            var householdsQuery = _context.Households.AsQueryable();

            if(date != "" && date != null)
            {
                try
                {
                    DateTime d = Convert.ToDateTime(date);
                    householdsQuery = householdsQuery.Where(x => x.RenewalDate <= d);
                }
                catch { }
            }

            if (exclInactive == true)
            {
                householdsQuery = householdsQuery.Where(x => x.RenewalDate >= DateTime.Today);
            }

            var households = from t1 in householdsQuery
                             orderby t1.RenewalDate
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Created_Date = t1.CreatedDate.ToShortDateString(),
                                 LICO_Verified = t1.LICOVerified,
                                 LICO_Verified_Date = t1.LICOVerifiedDate == null ? "" : t1.LICOVerifiedDate.Value.ToShortDateString(),
                                 Income_Total = t1.IncomeTotal,
                                 Renewal_Date = t1.RenewalDate.ToShortDateString()
                             };

            //How many rows?
            int numRows = households.Count();

            if (numRows > 0) //We have data
            {
                //Create a new spreadsheet from scratch.
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Membership Renewal Report");

                    //Note: Cells[row, column]
                    workSheet.Cells[3, 1].LoadFromCollection(households, true);

                    // Style columns
                    workSheet.Column(6).Style.Numberformat.Format = "###,##0.00";

                    //Totals
                    using (ExcelRange totals = workSheet.Cells[numRows + 4, 1])
                    {
                        totals.Value = "Total Memberships:";
                        totals.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        totals.Style.Font.Bold = true;
                    }
                    using (ExcelRange numMemberships = workSheet.Cells[numRows + 4, 2])
                    {
                        numMemberships.Value = households.Count();
                        numMemberships.Style.Numberformat.Format = "###,##0";
                        numMemberships.Style.Font.Bold = true;
                    }

                    //Autofit columns
                    workSheet.Cells.AutoFitColumns();

                    //Align columns to right
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 7])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Membership Renewal Report";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 7])
                    {
                        Rng.Merge = true; //Merge columns start and end range
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 18;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Since the time zone where the server is running can be different, adjust to local.
                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 7])
                    {
                        Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                            localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Specify the city
                    if(date != "" && date != null)
                    {
                        using (ExcelRange Rng = workSheet.Cells[2, 1])
                        {
                            Rng.Value = "Expiring by: " + date;
                            Rng.Style.Font.Bold = true; //Font should be bold
                            Rng.Style.Font.Size = 12;
                            Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }

                    //Ok, time to download the Excel
                    var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers["content-disposition"] = "attachment;  filename=RenewalReport.xlsx";
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.Body);
                        }
                    }
                    else
                    {
                        try
                        {
                            Byte[] theData = excel.GetAsByteArray();
                            string filename = "RenewalReport.xlsx";
                            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            return File(theData, mimeType, filename);
                        }
                        catch (Exception)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }

        public IActionResult DownloadHouseholdLocations(bool exclInactive, int cityID)
        {
            var householdsQuery = _context.Households
                .Include(x => x.City)
                .AsQueryable();

            string cityName = "";
            
            if(cityID != -1)
            {
                householdsQuery = householdsQuery.Where(x => x.CityID == cityID);
                cityName = _context.Cities.Where(c => c.ID == cityID).Select(c => c.CityName).FirstOrDefault();
            }

            if (exclInactive == true)
            {
                householdsQuery = householdsQuery.Where(x => x.RenewalDate >= DateTime.Today);
            }

            var households = from t1 in householdsQuery
                             orderby t1.MembershipNumber
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Income_Total = t1.IncomeTotal,
                                 Street_Number = t1.StreetNumber,
                                 Street_Name = t1.StreetName,
                                 Apartment_Number = t1.ApartmentNumber,
                                 City = t1.City.CityName,
                                 Postal_Code = t1.PostalCode.ToUpper()
                             };

            //How many rows?
            int numRows = households.Count();

            if (numRows > 0) //We have data
            {
                //Create a new spreadsheet from scratch.
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Household Location Report");

                    //Note: Cells[row, column]
                    workSheet.Cells[3, 1].LoadFromCollection(households, true);

                    // Style columns
                    workSheet.Column(3).Style.Numberformat.Format = "###,##0.00";

                    //Totals
                    using (ExcelRange totals = workSheet.Cells[numRows + 4, 1])
                    {
                        totals.Value = "Total Households:";
                        totals.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        totals.Style.Font.Bold = true;
                    }
                    using (ExcelRange numMemberships = workSheet.Cells[numRows + 4, 2])
                    {
                        numMemberships.Value = households.Count();
                        numMemberships.Style.Numberformat.Format = "###,##0";
                        numMemberships.Style.Font.Bold = true;
                    }

                    //Autofit columns
                    workSheet.Cells.AutoFitColumns();

                    //Align columns to right
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 8])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Household Location Report";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 8])
                    {
                        Rng.Merge = true; //Merge columns start and end range
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 18;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Since the time zone where the server is running can be different, adjust to local.
                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 8])
                    {
                        Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                            localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Specify the city
                    using (ExcelRange Rng = workSheet.Cells[2, 1])
                    {
                        Rng.Value = cityName == "" ? "All Cities" : "City: " + cityName;
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    //Ok, time to download the Excel
                    var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers["content-disposition"] = "attachment;  filename=LocationReport.xlsx";
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.Body);
                        }
                    }
                    else
                    {
                        try
                        {
                            Byte[] theData = excel.GetAsByteArray();
                            string filename = "LocationReport.xlsx";
                            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            return File(theData, mimeType, filename);
                        }
                        catch (Exception)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }

        public IActionResult DownloadHouseholdIncome(bool exclInactive, int lowRange, int highRange)
        {
            var householdsQuery = _context.Households.AsQueryable();

            if (highRange > 0)
            {
                householdsQuery = householdsQuery.Where(h => h.IncomeTotal >= Convert.ToInt32(lowRange) && h.IncomeTotal <= Convert.ToInt32(highRange));
            }

            if (exclInactive == true)
            {
                householdsQuery = householdsQuery.Where(x => x.RenewalDate >= DateTime.Today);
            }

            var households = from t1 in householdsQuery
                             orderby t1.IncomeTotal
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Income_Total = t1.IncomeTotal,
                                 LICO_Verified = t1.LICOVerified,
                                 LICO_Verified_Date = t1.LICOVerifiedDate == null ? "" : t1.LICOVerifiedDate.Value.ToShortDateString()
                             };

            //How many rows?
            int numRows = households.Count();

            if (numRows > 0) //We have data
            {
                //Create a new spreadsheet from scratch.
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Household Income Report");

                    //Note: Cells[row, column]
                    workSheet.Cells[3, 1].LoadFromCollection(households, true);

                    // Style columns
                    workSheet.Column(3).Style.Numberformat.Format = "###,##0.00";

                    //Totals
                    using (ExcelRange totals = workSheet.Cells[numRows + 4, 1])
                    {
                        totals.Value = "Total Households:";
                        totals.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        totals.Style.Font.Bold = true;
                    }
                    using (ExcelRange numMemberships = workSheet.Cells[numRows + 4, 2])
                    {
                        numMemberships.Value = households.Count();
                        numMemberships.Style.Numberformat.Format = "###,##0";
                        numMemberships.Style.Font.Bold = true;
                    }

                    //Autofit columns
                    workSheet.Cells.AutoFitColumns();

                    //Align columns to right
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 5])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Household Income Report";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 5])
                    {
                        Rng.Merge = true; //Merge columns start and end range
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 18;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Since the time zone where the server is running can be different, adjust to local.
                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 5])
                    {
                        Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                            localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Specify the income range
                    if (highRange > 0)
                    {
                        using (ExcelRange Rng = workSheet.Cells[2, 1])
                        {
                            Rng.Value = $"Income Range: {lowRange} - {highRange}";
                            Rng.Style.Font.Bold = true; //Font should be bold
                            Rng.Style.Font.Size = 12;
                            Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }

                    //Ok, time to download the Excel
                    var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers["content-disposition"] = "attachment;  filename=IncomeReport.xlsx";
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.Body);
                        }
                    }
                    else
                    {
                        try
                        {
                            Byte[] theData = excel.GetAsByteArray();
                            string filename = "IncomeReport.xlsx";
                            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            return File(theData, mimeType, filename);
                        }
                        catch (Exception)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }

        public IActionResult DownloadHouseholdVisits(bool exclInactive)
        {
            var householdsQuery = _context.Households
                .Include(x => x.City)
                .Include(x => x.Transactions)
                .AsQueryable();

            if (exclInactive == true)
            {
                householdsQuery = householdsQuery.Where(x => x.RenewalDate >= DateTime.Today);
            }

            var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day);

            var households = from t1 in householdsQuery
                             orderby t1.MembershipNumber
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Income_Total = t1.IncomeTotal,
                                 City = t1.City.CityName,
                                 Number_Of_Visits = t1.Transactions.Where(x => x.TransactionDate >= date).Count()
                             };

            //How many rows?
            int numRows = households.Count();

            if (numRows > 0) //We have data
            {
                //Create a new spreadsheet from scratch.
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Household Visits Report");

                    //Note: Cells[row, column]
                    workSheet.Cells[3, 1].LoadFromCollection(households, true);

                    // Style columns
                    workSheet.Column(3).Style.Numberformat.Format = "###,##0.00";

                    //Totals
                    using (ExcelRange totals = workSheet.Cells[numRows + 4, 1])
                    {
                        totals.Value = "Total Households:";
                        totals.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        totals.Style.Font.Bold = true;
                    }
                    using (ExcelRange numMemberships = workSheet.Cells[numRows + 4, 2])
                    {
                        numMemberships.Value = households.Count();
                        numMemberships.Style.Numberformat.Format = "###,##0";
                        numMemberships.Style.Font.Bold = true;
                    }

                    //Autofit columns
                    workSheet.Cells.AutoFitColumns();

                    //Align columns to right
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 5])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Household Visits Report";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 5])
                    {
                        Rng.Merge = true; //Merge columns start and end range
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 18;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Since the time zone where the server is running can be different, adjust to local.
                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 5])
                    {
                        Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                            localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Specify the date range
                    using (ExcelRange Rng = workSheet.Cells[2, 1])
                    {
                        Rng.Value = $"From {date.ToShortDateString()} to {DateTime.Today.ToShortDateString()}";
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    //Ok, time to download the Excel
                    var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers["content-disposition"] = "attachment;  filename=VisitsReport.xlsx";
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.Body);
                        }
                    }
                    else
                    {
                        try
                        {
                            Byte[] theData = excel.GetAsByteArray();
                            string filename = "VisitsReport.xlsx";
                            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            return File(theData, mimeType, filename);
                        }
                        catch (Exception)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }

        public IActionResult DownloadMemberAges(bool exclInactive, int lowRange, int highRange)
        {
            var membersQuery = _context.Members
                .Include(x => x.Gender)
                .AsQueryable();

            if (highRange > 0)
            {
                membersQuery = membersQuery.Where(m => DateTime.Today.Year - m.DOB.Year - ((DateTime.Today.Month < m.DOB.Month || (DateTime.Today.Month == m.DOB.Month && DateTime.Today.Day < m.DOB.Day) ? 1 : 0)) >= lowRange &&
                DateTime.Today.Year - m.DOB.Year - ((DateTime.Today.Month < m.DOB.Month || (DateTime.Today.Month == m.DOB.Month && DateTime.Today.Day < m.DOB.Day) ? 1 : 0)) <= highRange);
            }

            if (exclInactive == true)
            {
                membersQuery = membersQuery.Where(x => x.Household.RenewalDate >= DateTime.Today);
            }

            var members = from t1 in membersQuery
                             orderby t1.LastName, t1.FirstName
                             select new
                             {
                                 Name = t1.FullName,
                                 Age = t1.Age,
                                 Gender = t1.Gender.GenderType,
                                 Income = t1.IncomeAmount,
                                 Income_Verified = t1.IncomeVerified
                             };

            //How many rows?
            int numRows = members.Count();

            if (numRows > 0) //We have data
            {
                //Create a new spreadsheet from scratch.
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Member Age Report");

                    //Note: Cells[row, column]
                    workSheet.Cells[3, 1].LoadFromCollection(members, true);

                    // Style columns
                    workSheet.Column(4).Style.Numberformat.Format = "###,##0.00";

                    //Totals
                    using (ExcelRange totals = workSheet.Cells[numRows + 4, 1])
                    {
                        totals.Value = "Total Members:";
                        totals.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        totals.Style.Font.Bold = true;
                    }
                    using (ExcelRange numMemberships = workSheet.Cells[numRows + 4, 2])
                    {
                        numMemberships.Value = members.Count();
                        numMemberships.Style.Numberformat.Format = "###,##0";
                        numMemberships.Style.Font.Bold = true;
                    }

                    //Autofit columns
                    workSheet.Cells.AutoFitColumns();

                    //Align columns to right
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 5])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Member Age Report";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 5])
                    {
                        Rng.Merge = true; //Merge columns start and end range
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 18;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Since the time zone where the server is running can be different, adjust to local.
                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 5])
                    {
                        Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                            localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true; //Font should be bold
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Specify the age range
                    if (highRange > 0)
                    {
                        using (ExcelRange Rng = workSheet.Cells[2, 1])
                        {
                            Rng.Value = $"Age Range: {lowRange} - {highRange}";
                            Rng.Style.Font.Bold = true; //Font should be bold
                            Rng.Style.Font.Size = 12;
                            Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }

                    //Ok, time to download the Excel
                    var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                        using (var memoryStream = new MemoryStream())
                        {
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Headers["content-disposition"] = "attachment;  filename=AgeReport.xlsx";
                            excel.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.Body);
                        }
                    }
                    else
                    {
                        try
                        {
                            Byte[] theData = excel.GetAsByteArray();
                            string filename = "AgeReport.xlsx";
                            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            return File(theData, mimeType, filename);
                        }
                        catch (Exception)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }

        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }
    }
}
