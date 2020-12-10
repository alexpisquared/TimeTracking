using System;
using System.Diagnostics;

namespace TimeTracker.Common
{
  public static class HolidayProcessor // http://www.public-holidays.us/CA_EN_2019_Ontario
  {
    public static bool IsBizDay(this DateTime d) => !d.IsHolidayOrWeekend();
    public static bool IsGoodFriday(this DateTime d)
    { // Good Friday  Friday before Easter Sunday              
      if (d > new DateTime(2036, 4, 11))
        throw new NullReferenceException("AP: this is nice to know that you are still using this little app, but please note that beyond 2036 there is no Easter data.");
      else return
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
        || d == new DateTime(2036, 4, 11));
    }
    public static bool IsHolidayOrWeekend(this DateTime d)
    {
      if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
        return true;

      if (                                                    // UNMOVABLES:
        (d.Month == 01 && d.Day == 1) ||                      // New Year
        (d.Month == 07 && d.Day == 1) ||                      // Canada Day
        (d.Month == 12 && 24 < d.Day && d.Day < 27)           // Christmas
        )
        return true;

      if (d.DayOfWeek == DayOfWeek.Monday)
      {
        if (                                                    // mondays:
          (d.Month == 02 && 14 < d.Day && d.Day < 22) ||        // Family Day
          (d.Month == 05 && 17 < d.Day && d.Day < 25) ||        // Victoria Day
          (d.Month == 07 && d.Day == 2) ||                      // Canada Day was on Sunday
          (d.Month == 07 && d.Day == 3) ||                      // Canada Day was on Saturday
          (d.Month == 08 && d.Day < 8) ||                       // Civic Holiday
          (d.Month == 09 && d.Day < 8) ||                       // Labour Day
          (d.Month == 10 && 7 < d.Day && d.Day < 15) ||         // Thanksgiving
          (d.Month == 12 && d.Day == 27) ||                     // Christams  was on Saturday
          (d.Month == 12 && d.Day == 28)                        // Boxing Day was on Saturday
          )
          return true;
      }
      else if (d.DayOfWeek == DayOfWeek.Tuesday)
      {
        if (
          (d.Month == 12 && d.Day == 27) ||                     // Christams  was on Sunday
          (d.Month == 12 && d.Day == 28)                        // Boxing Day was on Sunday
          )
          return true;
      }
      else if (d.IsGoodFriday())
      {
        return true;
      }

      return false;
    }

    public static void Test()
    {
      Debug.WriteLine($">>            :   IsBizDay      IsHoliday ");
      for (var d = DateTime.Today.AddYears(-3); d < DateTime.Today.AddYears(4); d = d.AddDays(1))
      {
        if (DayOfWeek.Sunday < d.DayOfWeek && d.DayOfWeek < DayOfWeek.Saturday)
          if (d.IsBizDay() == d.IsHolidayOrWeekend())
          {
            Debug.WriteLine($">> {d:yyy-MMM-dd ddd} :   {d.IsBizDay()}    \t {d.IsHolidayOrWeekend()} ");
            Debug.Write("");
          }
          else Debug.WriteLine($">> {d:yyy-MMM-dd ddd}  ");
      }
    }
    public static bool IsBizDay_OLD(DateTime d)
    {
      var b =
        d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday //weekend
        && d != new DateTime(d.Year, 1, 1)    //New Year's Day  January 1               
        && d != new DateTime(d.Year, 7, 1)    //Canada Day
        && d != new DateTime(d.Year, 12, 25)  //Christmas Day  December 25
        && d != new DateTime(d.Year, 12, 26)  //Boxing Day  December 26  

        && d != new DateTime(2010, 12, 27)    //Christmas Day fell on Sat
        && d != new DateTime(2010, 12, 28)    //Boxing Day fell on Sun

        && d != new DateTime(2010, 2, 22)   //Family Day Third Monday in February
        && d != new DateTime(2011, 2, 21)
        && d != new DateTime(2012, 2, 20)
        && d != new DateTime(2013, 2, 18)
        && d != new DateTime(2014, 2, 17)
        && d != new DateTime(2015, 2, 16)
        && d != new DateTime(2016, 2, 15)
        && d != new DateTime(2017, 2, 20)
        && d != new DateTime(2018, 2, 19)
        && d != new DateTime(2019, 2, 18)
        && d != new DateTime(2020, 2, 17)
        && d != new DateTime(2021, 2, 15)
        && d != new DateTime(2022, 2, 21)
        && d != new DateTime(2023, 2, 20)
        && d != new DateTime(2024, 2, 19)
        && d != new DateTime(2025, 2, 17)
        && d != new DateTime(2026, 2, 16)
        && d != new DateTime(2027, 2, 15)
        && d != new DateTime(2028, 2, 21)
        && d != new DateTime(2029, 2, 19)

        && d != new DateTime(2010, 4, 6)    //Good Friday  Friday before Easter Sunday              
        && d != new DateTime(2011, 4, 22)
        && d != new DateTime(2012, 4, 6)
        && d != new DateTime(2013, 3, 29)
        && d != new DateTime(2014, 4, 18)
        && d != new DateTime(2015, 4, 3)
        && d != new DateTime(2016, 3, 25)
        && d != new DateTime(2017, 4, 14)
        && d != new DateTime(2018, 3, 30)
        && d != new DateTime(2019, 4, 19)
        && d != new DateTime(2020, 4, 10)
        && d != new DateTime(2021, 4, 02)
        && d != new DateTime(2022, 4, 15)
        && d != new DateTime(2023, 4, 07)
        && d != new DateTime(2024, 3, 29)
        && d != new DateTime(2025, 4, 18)
        && d != new DateTime(2026, 4, 03)
        && d != new DateTime(2027, 3, 26)
        && d != new DateTime(2028, 4, 14)
        && d != new DateTime(2029, 3, 30)

        && d != new DateTime(2010, 5, 24)   // Victoria Day
        && d != new DateTime(2011, 5, 23)
        && d != new DateTime(2012, 5, 21)
        && d != new DateTime(2013, 5, 20)
        && d != new DateTime(2014, 5, 19)
        && d != new DateTime(2015, 5, 18)
        && d != new DateTime(2016, 5, 23)
        && d != new DateTime(2017, 5, 22)
        && d != new DateTime(2018, 5, 21)
        && d != new DateTime(2019, 5, 20)
        && d != new DateTime(2020, 5, 18)
        && d != new DateTime(2021, 5, 24)
        && d != new DateTime(2022, 5, 23)
        && d != new DateTime(2023, 5, 22)
        && d != new DateTime(2024, 5, 20)
        && d != new DateTime(2025, 5, 19)
        && d != new DateTime(2026, 5, 18)
        && d != new DateTime(2027, 5, 24)
        && d != new DateTime(2028, 5, 22)
        && d != new DateTime(2029, 5, 21)

        && d != new DateTime(2023, 7, 3)    // Cnada Day on weekend 
        && d != new DateTime(2028, 7, 3)    // Cnada Day on weekend 
        && d != new DateTime(2029, 7, 2)    // Cnada Day on weekend 

        && d != new DateTime(2016, 8, 1)    //Civic Holiday 
        && d != new DateTime(2017, 8, 7)
        && d != new DateTime(2018, 8, 6)
        && d != new DateTime(2019, 8, 5)
        && d != new DateTime(2020, 8, 3)
        && d != new DateTime(2021, 8, 2)
        && d != new DateTime(2022, 8, 1)
        && d != new DateTime(2023, 8, 7)
        && d != new DateTime(2024, 8, 5)
        && d != new DateTime(2025, 8, 4)
        && d != new DateTime(2026, 8, 3)
        && d != new DateTime(2027, 8, 2)
        && d != new DateTime(2028, 8, 7)
        && d != new DateTime(2029, 8, 6)


        && d != new DateTime(2010, 9, 6)    //Labour Day First Monday of September   
        && d != new DateTime(2011, 9, 5)
        && d != new DateTime(2012, 9, 3)
        && d != new DateTime(2013, 9, 2)
        && d != new DateTime(2014, 9, 1)
        && d != new DateTime(2015, 9, 7)
        && d != new DateTime(2016, 9, 5)
        && d != new DateTime(2017, 9, 4)
        && d != new DateTime(2018, 9, 3)
        && d != new DateTime(2019, 9, 2)
        && d != new DateTime(2020, 9, 7)
        && d != new DateTime(2021, 9, 6)
        && d != new DateTime(2022, 9, 5)

        && d != new DateTime(2010, 10, 11)  //Thanksgiving Second Monday in October               
        && d != new DateTime(2011, 10, 10)
        && d != new DateTime(2012, 10, 8)
        && d != new DateTime(2013, 10, 14)
        && d != new DateTime(2014, 10, 13)
        && d != new DateTime(2015, 10, 12)
        && d != new DateTime(2016, 10, 10)
        && d != new DateTime(2017, 10, 9)
        && d != new DateTime(2018, 10, 8)
        && d != new DateTime(2019, 10, 14)
        && d != new DateTime(2020, 10, 12)
        && d != new DateTime(2021, 10, 11)
        && d != new DateTime(2022, 10, 10)

        && !d.IsGoodFriday();

      return b;
    }
  }
}
