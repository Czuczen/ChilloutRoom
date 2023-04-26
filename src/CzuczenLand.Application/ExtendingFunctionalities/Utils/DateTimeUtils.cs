using System;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - dateTime 
/// </summary>
public static class DateTimeUtils
{
    /// <summary>
    /// Zwraca datę pierwszego dnia tygodnia (poniedziałek) na podstawie podanej daty.
    /// </summary>
    /// <param name="fromDatetime">Data, dla której ma zostać zwrócona data pierwszego dnia tygodnia.</param>
    /// <returns>Data pierwszego dnia tygodnia (poniedziałek).</returns>
    public static DateTime GetDateOfFirstDayWeek(DateTime fromDatetime)
    {
        var diff = (7 + (fromDatetime.DayOfWeek - DayOfWeek.Monday)) % 7;
        var firstDayDateTime = fromDatetime.AddDays(-1 * diff).Date;

        return firstDayDateTime;
    }

    /// <summary>
    /// Konwertuje podaną liczbę sekund na łańcuch znaków reprezentujący czas w formacie DD:HH:MM:SS.
    /// </summary>
    /// <param name="seconds">Liczba sekund do skonwertowania.</param>
    /// <returns>Łańcuch znaków reprezentujący czas w formacie DD:HH:MM:SS.</returns>
    public static string ConvertSecondsToDdHhMmSs(double seconds)
    {
        var days = (int) Math.Floor(seconds / 86400);
        var daySeconds = days * 86400;
        var hours = (int) Math.Floor((seconds - daySeconds) / 3600);
        var hourSeconds = hours * 3600;
        var minutes = (int) Math.Floor((seconds - daySeconds - hourSeconds) / 60);
        var secs = (int) (seconds - daySeconds - hourSeconds - minutes * 60);
    
        return $"{days:00}:{hours:00}:{minutes:00}:{secs:00}";
    }
}