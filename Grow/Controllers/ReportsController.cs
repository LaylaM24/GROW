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
            else if (id == "MemberAges")
            {
                return RedirectToAction("MemberAges");
            }
            else if (id == "MemberGenders")
            {
                return RedirectToAction("MemberGenders");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> MembershipRenewal(int? page, int? pageSizeID,
            string actionButton, string sortDirection = "dec", string sortField = "Membership No.")
        {
            string[] sortOptions = new[] { "Membership No.", "No. Members", "Created Date", "LICO Verified", "LICO Verified Date", "Income Total", "Renewal Date" };

            var households = _context.Households.Where(x => x.RenewalDate <= DateTime.Today);

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
                        .OrderByDescending(m => m.LICOVerified);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.LICOVerified);
                }
            }
            else if (sortField == "LICO Verified Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerifiedDate);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.LICOVerifiedDate);
                }
            }
            else if (sortField == "Income Total")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
            }
            else if (sortField == "Renewal Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.RenewalDate);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.RenewalDate);
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
            string actionButton, string sortDirection = "dec", string sortField = "Membership No.", string City = "1")
        {
            string[] sortOptions = new[] { "Membership No.", "No. Members", "Total Income", "Street No.", "Street Name", "Apartment No.", "Postal Code" };

            var households = _context.Households.Where(h => h.CityID == Convert.ToInt32(City));

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
                        .OrderByDescending(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
            }
            else if (sortField == "Street No.")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.StreetNumber);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.StreetNumber);
                }
            }
            else if (sortField == "Street Name")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.StreetName);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.StreetName);
                }
            }
            else if (sortField == "Apartment No.")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.ApartmentNumber);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.ApartmentNumber);
                }
            }
            else if (sortField == "Postal Code")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.PostalCode);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.PostalCode);
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
            ViewData["City"] = new SelectList(_context.Cities
                .OrderBy(c => c.CityName), "ID", "CityName", City);

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID);
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Household>.CreateAsync(households.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        public async Task<IActionResult> HouseholdIncome(int? page, int? pageSizeID,
            string actionButton, string LowRange, string HighRange, string sortDirection = "dec", string sortField = "Membership No.")
        {
            string[] sortOptions = new[] { "Membership No.", "No. Members", "Total Income", "LICO Verified", "LICO Verified Date" };

            var households = _context.Households.AsQueryable();

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;

                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            if (LowRange != null && HighRange != null)
            {
                households = households.Where(h => h.IncomeTotal >= Convert.ToInt32(LowRange) && h.IncomeTotal <= Convert.ToInt32(HighRange));
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
                        .OrderByDescending(m => m.IncomeTotal);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.IncomeTotal);
                }
            }
            else if (sortField == "LICO Verified")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerified);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.LICOVerified);
                }
            }
            else if (sortField == "LICO Verified Date")
            {
                if (sortDirection == "asc")
                {
                    households = households
                        .OrderByDescending(m => m.LICOVerifiedDate);
                }
                else
                {
                    households = households
                        .OrderBy(m => m.LICOVerifiedDate);
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

        //public async Task<IActionResult> MemberAges()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> MemberGenders()
        //{
        //    return View();
        //}

        public IActionResult DownloadMembershipRenewal()
        {
            var households = from t1 in _context.Households.Where(x => x.RenewalDate <= DateTime.Today)
                             orderby t1.RenewalDate
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Created_Date = t1.CreatedDate,
                                 LICO_Verified = t1.LICOVerified,
                                 LICO_Verified_Date = t1.LICOVerifiedDate,
                                 Income_Total = t1.IncomeTotal,
                                 Renewal_Date = t1.RenewalDate
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

        public IActionResult DownloadHouseholdLocations(int cityID)
        {
            if (cityID == 0) return NotFound();

            string cityName = _context.Cities.Where(c => c.ID == cityID).Select(c => c.CityName).FirstOrDefault();

            var households = from t1 in _context.Households.Where(x => x.CityID == cityID)
                             orderby t1.MembershipNumber
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Income_Total = t1.IncomeTotal,
                                 Street_Number = t1.StreetNumber,
                                 Street_Name = t1.StreetName,
                                 Apartment_Number = t1.ApartmentNumber,
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
                    using (ExcelRange Rng = workSheet.Cells[4, 1, numRows + 4, 7])
                    {
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Add a title and timestamp at the top of the report
                    workSheet.Cells[1, 1].Value = "Household Location Report";
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
                    using (ExcelRange Rng = workSheet.Cells[2, 1])
                    {
                        Rng.Value = "City: " + cityName;
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

        public IActionResult DownloadHouseholdIncome(int lowRange, int highRange)
        {
            var allHouseholds = _context.Households.AsQueryable();

            if (highRange > 0)
            {
                allHouseholds = allHouseholds.Where(h => h.IncomeTotal >= Convert.ToInt32(lowRange) && h.IncomeTotal <= Convert.ToInt32(highRange));
            }

            var households = from t1 in allHouseholds
                             orderby t1.IncomeTotal
                             select new
                             {
                                 Membership_Number = t1.MembershipNumber,
                                 Number_Of_Members = t1.NumOfMembers,
                                 Income_Total = t1.IncomeTotal,
                                 LICO_Verified = t1.LICOVerified,
                                 LICO_Verified_Date = t1.LICOVerifiedDate
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
                    workSheet.Cells[1, 1].Value = "Household Location Report";
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

        private string ControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }
    }
}
