using Humanizer;
using System;

namespace UserManagementApp.Helpers
{
    public static class LogOutTime
    {
        public static string GetTime(DateTime dateTime)
        {
            // Ensure the input DateTime is treated as UTC
            var utcTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return utcTime.Humanize(); // Humanizer handles the relative time formatting
        }
    }
}