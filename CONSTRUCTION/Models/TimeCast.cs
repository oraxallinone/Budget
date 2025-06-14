using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONSTRUCTION.Models
{
    public static class TimeCast
    {
        public static DateTime ISTZone(DateTime? dateTimeNow)
        {
            DateTime newDate = dateTimeNow ?? DateTime.Now.AddYears(10);

            DateTime utcTime = newDate.ToUniversalTime();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
            return istTime;
        }

    }
}