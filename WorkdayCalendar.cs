class WorkdayCalendar : IWorkdayCalendar
{
    private List<DateTime> holidays = new List<DateTime>();

    private Dictionary<int, List<int>> recuringHolidays = new Dictionary<int, List<int>>();

    private TimeSpan StartTime;
    private TimeSpan EndTime;

    private bool IsRecurringHoliday(DateTime dateTime)
    {
        int month = dateTime.Month;
        int day = dateTime.Day;

        if (recuringHolidays.ContainsKey(month) && recuringHolidays[month].Contains(day))
        {
            return true;
        }
        return false;
    }

    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {

        DateTime adjustedDateTime = startDate;
        string format = "dd-MM-yyyy HH:mm";
        bool isAdding = incrementInWorkdays > 0;

        int minutes = (EndTime - StartTime).Hours * 60 + (EndTime - StartTime).Minutes;

        decimal fraction = incrementInWorkdays % 1;

        decimal adjustedMinutes = minutes * fraction;
        // Console.WriteLine(adjustedDateTime.ToString(format));
        // Console.WriteLine(StartTime.ToString());
        // Console.WriteLine(adjustedDateTime.TimeOfDay < StartTime);
        if (adjustedDateTime.TimeOfDay < StartTime)
        {

            adjustedDateTime = isAdding ? adjustedDateTime.Date.Add(StartTime) : adjustedDateTime.Date.Add(EndTime).AddDays(-1);
        }

        if (adjustedDateTime.TimeOfDay > EndTime)
        {
            adjustedDateTime = isAdding ? adjustedDateTime.Date.Add(StartTime).AddDays(1) : adjustedDateTime.Date.Add(EndTime);
        }

        adjustedDateTime = adjustedDateTime.AddMinutes((double)adjustedMinutes);
        // Console.Write(adjustedMinutes);
        // Console.WriteLine(adjustedDateTime.ToString(format));

        int daysToAdd = Math.Abs((int)incrementInWorkdays);
        Console.WriteLine(adjustedDateTime.ToString(format));
        Console.WriteLine("Number of iterations: " + daysToAdd);
        for (int i = 0; i < daysToAdd; i++)
        {
            var newDate = adjustedDateTime.AddDays(isAdding ? 1 : -1);
            Console.WriteLine("-----------");
            Console.WriteLine("Iteration " + i);
            Console.WriteLine("Loop Start. Adding 1 day: " + newDate.ToString(format));
            do
            {
                if (newDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    newDate = newDate.AddDays(isAdding ? 2 : -1);
                    Console.WriteLine("It's saturday. Adding 2 days: " + newDate.ToString(format));
                }
                else if (newDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    newDate = newDate.AddDays(isAdding ? 1 : -2);
                    Console.WriteLine("It's sunday. Adding 1 day: " + newDate.ToString(format));
                }

                if (IsRecurringHoliday(newDate) || holidays.Contains(newDate.Date))
                {
                    newDate = newDate.AddDays(isAdding ? 1 : -1);
                    Console.WriteLine("It's holiday. Adding 1 day: " + newDate.ToString(format));
                    continue;
                }
                break;
            } while (true);
            Console.WriteLine("Loop End. New value: " + newDate.ToString(format));
            adjustedDateTime = newDate;
        }



        return adjustedDateTime;
    }

    public void SetHoliday(DateTime date)
    {
        holidays.Add(date);
    }

    public void SetRecurringHoliday(int month, int day)
    {
        if (recuringHolidays.ContainsKey(month))
        {
            recuringHolidays[month].Add(day);
            return;
        }
        recuringHolidays.Add(month, new List<int> { day });
    }

    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
    {
        StartTime = new TimeSpan(startHours, startMinutes, 0);
        EndTime = new TimeSpan(stopHours, stopMinutes, 0);
    }
}