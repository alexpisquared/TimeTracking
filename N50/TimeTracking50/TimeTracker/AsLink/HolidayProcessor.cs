using System;
using System.Diagnostics;

namespace TimeTracker.AsLink;

public static class HolidayProcessor // http://www.public-holidays.us/CA_EN_2019_Ontario
{
  public static bool IsBizDay(this DateTime d) => !d.IsHolidayOrWeekend();
  public static bool IsGoodFriday(this DateTime d)
  { // Good Friday  Friday before Easter Sunday              
    //if (d > new DateTime(2099, 4, 12))        throw new NullReferenceException("AP: this is nice to know that you are still using this little app, but please note that beyond 2036 there is no Easter data.");      else 
    return
      d.DayOfWeek == DayOfWeek.Friday
      && (d == new DateTime(2010, 4, 6)
      || d == new DateTime(2011, 4, 22)
      || d == new DateTime(2012, 4, 6)
      || d == new DateTime(2013, 3, 29)
      || d == new DateTime(2014, 4, 18)
      || d == new DateTime(2015, 4, 3)
      || d == new DateTime(2016, 3, 25)
      || d == new DateTime(2017, 4, 14)
      || d == new DateTime(2018, 3, 30)
      || d == new DateTime(2019, 4, 19)
      || d == new DateTime(2020, 4, 10)
      || d == new DateTime(2021, 4, 02)
      || d == new DateTime(2022, 4, 15)
      || d == new DateTime(2023, 4, 07)
      || d == new DateTime(2024, 3, 29)
      || d == new DateTime(2025, 4, 18)
      || d == new DateTime(2026, 4, 03)
      || d == new DateTime(2027, 3, 26)
      || d == new DateTime(2028, 4, 14)
      || d == new DateTime(2029, 3, 30)
      || d == new DateTime(2030, 4, 19)
      || d == new DateTime(2031, 4, 11)
      || d == new DateTime(2032, 3, 26)
      || d == new DateTime(2033, 4, 15)
      || d == new DateTime(2034, 4, 07)
      || d == new DateTime(2035, 3, 23)
      || d == new DateTime(2036, 4, 13 - 2)
      || d == new DateTime(2037, 4, 5 - 2)
      || d == new DateTime(2038, 4, 25 - 2)
      || d == new DateTime(2039, 4, 10 - 2)
      || d == new DateTime(2040, 3,30)
      || d == new DateTime(2041, 4, 21 - 2)
      || d == new DateTime(2042, 4, 6 - 2)
      || d == new DateTime(2043, 3, 29 - 2)
      || d == new DateTime(2044, 4, 17 - 2)
      || d == new DateTime(2045, 4, 9 - 2)
      || d == new DateTime(2046, 3, 25 - 2)
      || d == new DateTime(2047, 4, 14 - 2)
      || d == new DateTime(2048, 4, 5 - 2)
      || d == new DateTime(2049, 4, 18 - 2)
      || d == new DateTime(2050, 4, 10 - 2)
      || d == new DateTime(2051, 3,31)
      || d == new DateTime(2052, 4, 21 - 2)
      || d == new DateTime(2053, 4, 6 - 2)
      || d == new DateTime(2054, 3, 29 - 2)
      || d == new DateTime(2055, 4, 18 - 2)
      || d == new DateTime(2056, 3,31)
      || d == new DateTime(2057, 4, 22 - 2)
      || d == new DateTime(2058, 4, 14 - 2)
      || d == new DateTime(2059, 3, 30 - 2)
      || d == new DateTime(2060, 4, 18 - 2)
      || d == new DateTime(2061, 4, 10 - 2)
      || d == new DateTime(2062, 3, 26 - 2)
      || d == new DateTime(2063, 4, 15 - 2)
      || d == new DateTime(2064, 4, 6 - 2)
      || d == new DateTime(2065, 3, 29 - 2)
      || d == new DateTime(2066, 4, 11 - 2)
      || d == new DateTime(2067, 4, 3 - 2)
      || d == new DateTime(2068, 4, 22 - 2)
      || d == new DateTime(2069, 4, 14 - 2)
      || d == new DateTime(2070, 3, 30 - 2)
      || d == new DateTime(2071, 4, 19 - 2)
      || d == new DateTime(2072, 4, 10 - 2)
      || d == new DateTime(2073, 3, 26 - 2)
      || d == new DateTime(2074, 4, 15 - 2)
      || d == new DateTime(2075, 4, 7 - 2)
      || d == new DateTime(2076, 4, 19 - 2)
      || d == new DateTime(2077, 4, 11 - 2)
      || d == new DateTime(2078, 4, 3 - 2)
      || d == new DateTime(2079, 4, 23 - 2)
      || d == new DateTime(2080, 4, 7 - 2)
      || d == new DateTime(2081, 3, 30 - 2)
      || d == new DateTime(2082, 4, 19 - 2)
      || d == new DateTime(2083, 4, 4 - 2)
      || d == new DateTime(2084, 3, 26 - 2)
      || d == new DateTime(2085, 4, 15 - 2)
      || d == new DateTime(2086, 3, 31 - 2)
      || d == new DateTime(2087, 4, 20 - 2)
      || d == new DateTime(2088, 4, 11 - 2)
      || d == new DateTime(2089, 4, 3 - 2)
      || d == new DateTime(2090, 4, 16 - 2)
      || d == new DateTime(2091, 4, 8 - 2)
      || d == new DateTime(2092, 3, 30 - 2)
      || d == new DateTime(2093, 4, 12 - 2)
      || d == new DateTime(2094, 4, 4 - 2)
      || d == new DateTime(2095, 4, 24 - 2)
      || d == new DateTime(2096, 4, 15 - 2)
      || d == new DateTime(2097, 3, 31 - 2)
      || d == new DateTime(2098, 4, 20 - 2)
      || d == new DateTime(2099, 4, 12 - 2)
      );
  }
  public static bool IsHolidayOrWeekend(this DateTime d)
  {
    if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
      return true;

    if (                                                  // UNMOVABLES:
      d.Month == 01 && d.Day == 1 ||                      // New Year
      d.Month == 07 && d.Day == 1 ||                      // Canada Day
      d.Month == 12 && 24 < d.Day && d.Day < 27           // Christmas
      )
      return true;

    if (d.DayOfWeek == DayOfWeek.Monday)
    {
      if (                                                    // mondays:
        d.Month == 01 && d.Day == 2 ||                      // New Year was on Sunday
        d.Month == 01 && d.Day == 3 ||                      // New Year was on Saturday
        d.Month == 02 && 14 < d.Day && d.Day < 22 ||        // Family Day
        d.Month == 05 && 17 < d.Day && d.Day < 25 ||        // Victoria Day
        d.Month == 07 && d.Day == 2 ||                      // Canada Day was on Sunday
        d.Month == 07 && d.Day == 3 ||                      // Canada Day was on Saturday
        d.Month == 08 && d.Day < 8 ||                       // Civic Holiday
        d.Month == 09 && d.Day < 8 ||                       // Labour Day
        d.Month == 10 && 7 < d.Day && d.Day < 15 ||         // Thanksgiving
        d.Month == 12 && d.Day == 27 ||                     // Christams  was on Saturday
        d.Month == 12 && d.Day == 28                        // Boxing Day was on Saturday
        )
        return true;
    }
    else if (d.DayOfWeek == DayOfWeek.Tuesday)
    {
      if (
        d.Month == 12 && d.Day == 27 ||                     // Christams  was on Sunday
        d.Month == 12 && d.Day == 28                        // Boxing Day was on Sunday
        )
        return true;
    }
    else if (d.DayOfWeek == DayOfWeek.Friday && d.IsGoodFriday())
      return true;

    return false;
  }

  public static void Test()
  {
    Debug.WriteLine(" yyyy   New Year     Family Day   Good Friday  Victoria Day Canada Day   Labour Day   Oct-09 Mon   Dec-25 Mon   Dec-25 Tue   Dec-26 Tue  ");

    for (var y = 2022; y < 2033; y++)
    {
      Debug.Write($"\n {y}  ");

      for (var d = new DateTime(y, 1, 1); d < new DateTime(y + 1, 1, 1); d = d.AddDays(1))
        if (DayOfWeek.Sunday < d.DayOfWeek && d.DayOfWeek < DayOfWeek.Saturday)
          if (d.IsHolidayOrWeekend())
            Debug.Write($" {d:MMM-dd ddd}  ");
    }
  }
}
