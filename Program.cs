static void PrintIncrement(DateTime start, decimal increment, DateTime result)
{
  Console.WriteLine(
      start.ToString("dd-MM-yyyy HH:mm") +
      " with an addition of " +
      increment +
      " work days is " +
      result.ToString("dd-MM-yyyy HH:mm"));
}

IWorkdayCalendar calendar = new WorkdayCalendar();
calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
calendar.SetRecurringHoliday(5, 17);
calendar.SetHoliday(new DateTime(2004, 5, 27));


var start1 = new DateTime(2004, 5, 24, 18, 5, 0);
decimal increment1 = -5.5m;
var example1 = calendar.GetWorkdayIncrement(start1, increment1);
PrintIncrement(start1, increment1, example1);
