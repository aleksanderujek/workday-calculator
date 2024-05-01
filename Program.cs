// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
IWorkdayCalendar calendar = new WorkdayCalendar();
calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
calendar.SetRecurringHoliday(5, 17);
calendar.SetHoliday(new DateTime(2004, 5, 27));
string format = "dd-MM-yyyy HH:mm";
var start1 = new DateTime(2004, 5, 24, 18, 5, 0);
decimal increment1 = -5.5m;
var example1 = calendar.GetWorkdayIncrement(start1, increment1);
Console.WriteLine(
  start1.ToString(format) +
  " with an addition of " +
  increment1 +
  " work days is " +
  example1.ToString(format));

var start2 = new DateTime(2004, 5, 24, 19, 3, 0);
decimal increment2 = 44.723656m;
var example2 = calendar.GetWorkdayIncrement(start2, increment2);
Console.WriteLine(
  start2.ToString(format) +
  " with an addition of " +
  increment2 +
  " work days is " +
  example2.ToString(format));

var start3 = new DateTime(2004, 5, 24, 18, 3, 0);
decimal increment3 = -6.7470217m;
var example3 = calendar.GetWorkdayIncrement(start3, increment3);
Console.WriteLine(
  start3.ToString(format) +
  " with an addition of " +
  increment3 +
  " work days is " +
  example3.ToString(format));

var start4 = new DateTime(2004, 5, 24, 8, 3, 0);
decimal increment4 = 12.782709m;
var example4 = calendar.GetWorkdayIncrement(start4, increment4);
Console.WriteLine(
  start4.ToString(format) +
  " with an addition of " +
  increment4 +
  " work days is " +
  example4.ToString(format));

var start5 = new DateTime(2004, 5, 24, 7, 3, 0);
decimal increment5 = 8.276628m;
var example5 = calendar.GetWorkdayIncrement(start5, increment5);
Console.WriteLine(
  start5.ToString(format) +
  " with an addition of " +
  increment5 +
  " work days is " +
  example5.ToString(format));