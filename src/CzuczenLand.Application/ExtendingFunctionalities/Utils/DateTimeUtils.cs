using System;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class DateTimeUtils
{
    public static DateTime GetDateOfFirstDayWeek(DateTime fromDatetime)
    {
        var diff = (7 + (fromDatetime.DayOfWeek - DayOfWeek.Monday)) % 7;
        var firstDayDateTime = fromDatetime.AddDays(-1 * diff).Date;

        return firstDayDateTime;
    }

    public static string ConvertSecondsToDdHhMmSs(double seconds)
    {
        var days = (int)Math.Floor(seconds / 86400);
        var daySeconds = days * 86400;
        var hours = (int)Math.Floor((seconds - daySeconds) / 3600);
        var hourSeconds = hours * 3600;
        var minutes = (int)Math.Floor((seconds - daySeconds - hourSeconds) / 60);
        var secs = (int)(seconds - daySeconds - hourSeconds - minutes * 60);
    
        return $"{days:00}:{hours:00}:{minutes:00}:{secs:00}";
    }
}