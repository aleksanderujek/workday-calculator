class WorkdayCalendar : IWorkdayCalendar
{
    private List<DateTime> holidays = new List<DateTime>();

    private Dictionary<int, List<int>> recuringHolidays = new Dictionary<int, List<int>>();

    private TimeSpan StartTime;
    private TimeSpan EndTime;

    private WorkdayValidator validator = new WorkdayValidator();

    /// <summary>
    /// Sets the start and stop time for the workday.
    /// </summary>
    /// <param name="startHours">The hours component of the start time.</param>
    /// <param name="startMinutes">The minutes component of the start time.</param>
    /// <param name="stopHours">The hours component of the stop time.</param>
    /// <param name="stopMinutes">The minutes component of the stop time.</param>
    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
    {
        validator.ValidateHour(startHours, "start");
        validator.ValidateMinute(startMinutes, "start");
        validator.ValidateHour(stopHours, "stop");
        validator.ValidateMinute(stopMinutes, "stop");

        TimeSpan startTime = new TimeSpan(startHours, startMinutes, 0);
        TimeSpan endTime = new TimeSpan(stopHours, stopMinutes, 0);

        validator.ValidateStartTimeToEndTime(startTime, endTime);

        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    /// Calculates the date and time of a workday increment based on the start date and the number of workdays to increment.
    /// </summary>
    /// <param name="startDate">The start date of the increment.</param>
    /// <param name="incrementInWorkdays">The number of workdays to increment.</param>
    /// <returns>The date and time of the workday increment.</returns>
    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {
        validator.ValidateDateTimeIfEmpty(startDate);
        validator.ValidateTimeSpanIfEmpty(StartTime, EndTime);
        validator.ValidateStartTimeToEndTime(StartTime, EndTime);

        int workdayMinutes = (EndTime - StartTime).Hours * 60 + (EndTime - StartTime).Minutes;

        bool isPositiveIncrement = incrementInWorkdays > 0;
        decimal fraction = incrementInWorkdays % 1;
        decimal adjustedMinutes = workdayMinutes * fraction;

        startDate = AdjustStartDateBasedOnWorkHours(startDate, isPositiveIncrement);

        startDate = AdjustWorkDayToIncrementMinutes(startDate, isPositiveIncrement, adjustedMinutes);

        int workDays = Math.Abs((int)incrementInWorkdays);
        if (workDays == 0)
        {
            startDate = CalculateIncrement(startDate, isPositiveIncrement);
            return startDate;
        }

        int increment = isPositiveIncrement ? 1 : -1;
        for (int i = 0; i < workDays; i++)
        {
            startDate = CalculateIncrement(startDate.AddDays(increment), isPositiveIncrement);
        }
        return startDate;
    }

    /// <summary>
    /// Sets a new holiday date in the workday calendar.
    /// </summary>
    /// <param name="date">The date to set as a holiday.</param>
    public void SetHoliday(DateTime date)
    {
        if (date == DateTime.MinValue)
        {
            throw new ArgumentException("New holiday date cannot be empty.");
        }
        holidays.Add(date);
    }

    /// <summary>
    /// Sets a recurring holiday on the specified month and day.
    /// </summary>
    /// <param name="month">The month of the recurring holiday.</param>
    /// <param name="day">The day of the recurring holiday.</param>
    public void SetRecurringHoliday(int month, int day)
    {
        validator.ValidateDate(month, day);

        if (recuringHolidays.ContainsKey(month))
        {
            recuringHolidays[month].Add(day);
            return;
        }

        recuringHolidays.Add(month, new List<int> { day });
    }

    #region Private Methods

    /// <summary>
    /// Checks if the specified date is a recurring holiday.
    /// </summary>
    /// <param name="dateTime">The date to check.</param>
    /// <returns><c>true</c> if the specified date is a recurring holiday; otherwise <c>false</c>.</returns>
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


    /// <summary>
    /// Adjusts the work day by incrementing or decrementing the specified <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="workDay">The <see cref="DateTime"/> value representing the work day.</param>
    /// <param name="isPositiveIncrement">A boolean value indicating whether the increment is positive or negative.</param>
    /// <param name="fraction">The fraction by which to increment or decrement the work day.</param>
    /// <returns>The adjusted <see cref="DateTime"/> value.</returns>
    private DateTime AdjustWorkDayToIncrementMinutes(DateTime workDay, bool isPositiveIncrement, decimal fraction)
    {
        bool isOutsideWorkingHours = isPositiveIncrement ? (double)fraction > (EndTime - workDay.TimeOfDay).TotalMinutes : Math.Abs((double)fraction) > (workDay.TimeOfDay - StartTime).TotalMinutes;
        if (isOutsideWorkingHours)
        {
            double usedHours = isPositiveIncrement ? (EndTime - workDay.TimeOfDay).TotalMinutes : (workDay.TimeOfDay - StartTime).TotalMinutes;
            double difference = isPositiveIncrement ? (double)fraction - usedHours : (double)fraction + usedHours;
            workDay = isPositiveIncrement ? workDay.Date.Add(EndTime).AddMinutes(1) : workDay.Date.Add(StartTime).AddMinutes(-1);
            workDay = AdjustStartDateBasedOnWorkHours(workDay, isPositiveIncrement);
            workDay = workDay.AddMinutes((int)difference);
        }
        else
        {
            workDay = workDay.AddMinutes((int)fraction);
        }
        return workDay;
    }


    /// <summary>
    /// Adjusts the start date for an increment based on the specified parameters.
    /// </summary>
    /// <param name="startDate">The start date to adjust.</param>
    /// <param name="isPositiveIncrement">A boolean value indicating whether the increment is positive or negative.</param>
    /// <returns>The adjusted start date.</returns>
    private DateTime AdjustStartDateBasedOnWorkHours(DateTime startDate, bool isPositiveIncrement)
    {
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
        return startDate;
    }

    /// <summary>
    /// Calculates the increment for the specified <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> value to calculate the increment for.</param>
    /// <param name="isPositiveIncrement">A boolean value indicating whether the increment is positive or negative.</param>
    /// <returns>The incremented <see cref="DateTime"/> value.</returns>
    private DateTime CalculateIncrement(DateTime dateTime, bool isPositiveIncrement)
    {
        IDayIncrementStrategy strategy = dateTime.DayOfWeek switch
        {
            DayOfWeek.Saturday => new SaturdayStrategy(),
            DayOfWeek.Sunday => new SundayStrategy(),
            _ => IsRecurringHoliday(dateTime) || holidays.Contains(dateTime.Date) ? new HolidayStrategy() : new DefaultStrategy()
        };

        DateTime incrementedDate = strategy.CalculcateNewWorkday(dateTime, isPositiveIncrement);

        if (incrementedDate != dateTime)
        {
            return CalculateIncrement(incrementedDate, isPositiveIncrement);
        }

        return incrementedDate;
    }
    #endregion
}