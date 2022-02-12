using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Utilities
{
    public static class PageSizeHelper
    {
        public static int SetPageSize(HttpContext httpContext, int? pageSizeID)
        {
            int pageSize;
            if (pageSizeID.HasValue)
            {
                pageSize = pageSizeID.GetValueOrDefault();
                CookieHelper.CookieSet(httpContext, "pageSizeValue", pageSize.ToString(), 30);
            }
            else
            {
                pageSize = Convert.ToInt32(httpContext.Request.Cookies["pageSizeValue"]);
            }
            return (pageSize == 0) ? 5 : pageSize;
        }
        
        public static SelectList PageSizeList(int? pageSize)
        {
            return new SelectList(new[] { "3", "5", "10", "20", "30", "40", "50", "100", "500" }, pageSize.ToString());
        }
    }
}
