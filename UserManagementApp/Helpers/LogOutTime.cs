using System;
using Humanizer;

namespace UserManagementApp.Helpers
{
    public static class LogOutTime
    {
        public static string GetTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local).Humanize();
        }
    }
}
