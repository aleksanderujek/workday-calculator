class WorkdayCalendar : IWorkdayCalendar
{
    private List<DateTime> holidays = new List<DateTime>();

    private Dictionary<int, List<int>> recuringHolidays = new Dictionary<int, List<int>>();

    private TimeSpan StartTime;
    private TimeSpan EndTime;

    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
    {
        ValidateHour(startHours, "start");
        ValidateMinute(startMinutes, "start");
        ValidateHour(stopHours, "stop");
        ValidateMinute(stopMinutes, "stop");

        TimeSpan startTime = new TimeSpan(startHours, startMinutes, 0);
        TimeSpan endTime = new TimeSpan(stopHours, stopMinutes, 0);

        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {
        if (startDate == DateTime.MinValue)
        {
            throw new ArgumentException("Start date cannot be empty.");
        }

        int workdayMinutes;
        if (StartTime == default || EndTime == default)
        {
            throw new InvalidOperationException("Workday start and stop times must be set before calculating workday increment.");
        }
        else
        {
            workdayMinutes = (EndTime - StartTime).Hours * 60 + (EndTime - StartTime).Minutes;
        }

        decimal fraction = incrementInWorkdays % 1;
        decimal adjustedMinutes = workdayMinutes * fraction;

        bool isPositiveIncrement = incrementInWorkdays > 0;

        if (startDate.TimeOfDay < StartTime)
        {
            // If isPositiveIncrement is true, set startDate time as workday's StartTime.
            // Otherwise, subtract one day from startDate and set time as workday's EndTime.
            startDate = isPositiveIncrement ? startDate.Date.Add(StartTime) : startDate.Date.Add(EndTime).AddDays(-1);
        }

        if (startDate.TimeOfDay > EndTime)
        {
            // If isPositiveIncrement is true, add one day to startDate and set time as workday's StartTime.
            // Otherwise, set startDate time as workday's EndTime.
            startDate = isPositiveIncrement ? startDate.Date.Add(StartTime).AddDays(1) : startDate.Date.Add(EndTime);
        }

        startDate = startDate.AddMinutes((int)adjustedMinutes);

        int daysToAdd = Math.Abs((int)incrementInWorkdays);

        if (daysToAdd == 0)
        {
            startDate = CalculateIncrement(startDate, isPositiveIncrement);
        }

        for (int i = 0; i < daysToAdd; i++)
        {
            var newDate = startDate.AddDays(isPositiveIncrement ? 1 : -1);
            newDate = CalculateIncrement(newDate, isPositiveIncrement);
            startDate = newDate;
        }

        return startDate;
    }

    public void SetHoliday(DateTime date)
    {
        if (date == DateTime.MinValue)
        {
            throw new ArgumentException("New holiday date cannot be empty.");
        }
        holidays.Add(date);
    }

    public void SetRecurringHoliday(int month, int day)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentException("Invalid month. Month must be between 1 and 12.");
        }

        if (day < 1 || day > 31)
        {
            throw new ArgumentException("Invalid day. Day must be between 1 and 31.");
        }

        if (!IsValidDate(month, day))
        {
            throw new ArgumentException("Invalid date. The specified date does not exist.");
        }

        if (recuringHolidays.ContainsKey(month))
        {
            recuringHolidays[month].Add(day);
            return;
        }

        recuringHolidays.Add(month, new List<int> { day });
    }

    #region Private Methods
    private bool IsValidDate(int month, int day)
    {
        try
        {
            DateTime date = new DateTime(DateTime.Now.Year, month, day);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    private void ValidateHour(int hour, string label)
    {
        if (hour < 0 || hour > 24)
        {
            throw new ArgumentException($"Invalid {label} hour. {label} hour must be between 0 and 24.");
        }
    }

    private void ValidateMinute(int minute, string label)
    {
        if (minute < 0 || minute > 59)
        {
            throw new ArgumentException($"Invalid {label} minute. {label} minute must be between 0 and 59.");
        }
    }

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

    private DateTime CalculateIncrement(DateTime dateTime, bool isAdding)
    {
        IDayIncrementStrategy strategy = dateTime.DayOfWeek switch
        {
            DayOfWeek.Saturday => new SaturdayStrategy(),
            DayOfWeek.Sunday => new SundayStrategy(),
            _ => IsRecurringHoliday(dateTime) || holidays.Contains(dateTime.Date) ? new HolidayStrategy() : new DefaultStrategy()
        };

        DateTime incrementedDate = strategy.CalculcateNewWorkday(dateTime, isAdding);

        if (incrementedDate != dateTime)
        {
            return CalculateIncrement(incrementedDate, isAdding);
        }

        return incrementedDate;
    }

    #endregion
}