using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONSTRUCTION.Models
{
    public static class GlobalDate
    {
        public static DateTime ISTZone(DateTime dateTimeNow)
        {
            DateTime utcTime = dateTimeNow.ToUniversalTime();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
            return istTime;
        }
    }
}