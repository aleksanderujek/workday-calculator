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

var start2 = new DateTime(2004, 5, 24, 19, 3, 0);
decimal increment2 = 44.723656m;
var example2 = calendar.GetWorkdayIncrement(start2, increment2);
PrintIncrement(start2, increment2, example2);

var start3 = new DateTime(2004, 5, 24, 18, 3, 0);
decimal increment3 = -6.7470217m;
var example3 = calendar.GetWorkdayIncrement(start3, increment3);
PrintIncrement(start3, increment3, example3);

var start4 = new DateTime(2004, 5, 24, 8, 3, 0);
decimal increment4 = 12.782709m;
var example4 = calendar.GetWorkdayIncrement(start4, increment4);
PrintIncrement(start4, increment4, example4);

var start5 = new DateTime(2004, 5, 24, 7, 3, 0);
decimal increment5 = 8.276628m;
var example5 = calendar.GetWorkdayIncrement(start5, increment5);
PrintIncrement(start5, increment5, example5);

var start6 = new DateTime(2004, 5, 24, 15, 7, 0);
decimal increment6 = 0.25m;
var example6 = calendar.GetWorkdayIncrement(start6, increment6);
PrintIncrement(start6, increment6, example6);

var start7 = new DateTime(2004, 5, 24, 4, 0, 0);
decimal increment7 = 0.5m;
var example7 = calendar.GetWorkdayIncrement(start7, increment7);
PrintIncrement(start7, increment7, example7);

var start8 = new DateTime(2004, 5, 24, 8, 0, 0);
decimal increment8 = 2.75m;
var example8 = calendar.GetWorkdayIncrement(start8, increment8);
PrintIncrement(start8, increment8, example8);