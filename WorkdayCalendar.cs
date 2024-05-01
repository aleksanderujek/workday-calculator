class WorkdayCalendar : IWorkdayCalendar
{
    private List<DateTime> holidays = new List<DateTime>();

    private Dictionary<int, List<int>> recuringHolidays = new Dictionary<int, List<int>>();

    private TimeSpan StartTime;
    private TimeSpan EndTime;

    /// <summary>
    /// Sets the start and stop time for the workday.
    /// </summary>
    /// <param name="startHours">The hours component of the start time.</param>
    /// <param name="startMinutes">The minutes component of the start time.</param>
    /// <param name="stopHours">The hours component of the stop time.</param>
    /// <param name="stopMinutes">The minutes component of the stop time.</param>
    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
    {
        ValidateHour(startHours, "start");
        ValidateMinute(startMinutes, "start");
        ValidateHour(stopHours, "stop");
        ValidateMinute(stopMinutes, "stop");

        TimeSpan startTime = new TimeSpan(startHours, startMinutes, 0);
        TimeSpan endTime = new TimeSpan(stopHours, stopMinutes, 0);

        ValidateStartTimeToEndTime(startTime, endTime);

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
        if (startDate == DateTime.MinValue)
        {
            throw new ArgumentException("Start date cannot be empty.");
        }

        if (StartTime == default || EndTime == default)
        {
            throw new InvalidOperationException("Workday start and stop times must be set before calculating workday increment.");
        }

        ValidateStartTimeToEndTime(StartTime, EndTime);
        int workdayMinutes = (EndTime - StartTime).Hours * 60 + (EndTime - StartTime).Minutes;

        bool isPositiveIncrement = incrementInWorkdays > 0;
        decimal fraction = incrementInWorkdays % 1;
        decimal adjustedMinutes = workdayMinutes * fraction;

        startDate = AdjustStartDateBasedOnWorkHours(startDate, isPositiveIncrement);

        startDate = startDate.AddMinutes((int)adjustedMinutes);

        int workDays = Math.Abs((int)incrementInWorkdays);
        if (workDays == 0)
        {
            // If increment is less than 1 workday, keep the time difference
            startDate = AdjustStartDateBasedOnWorkHours(startDate, isPositiveIncrement, includeDifference: true);
            startDate = CalculateIncrement(startDate, isPositiveIncrement);
        }
        else
        {
            int increment = isPositiveIncrement ? 1 : -1;
            for (int i = 0; i < workDays; i++)
            {
                startDate = CalculateIncrement(startDate.AddDays(increment), isPositiveIncrement);
            }
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
    /// <summary>
    /// Checks if the specified month and day form a valid date.
    /// </summary>
    /// <param name="month">The month component of the date.</param>
    /// <param name="day">The day component of the date.</param>
    /// <returns><c>true</c> if the specified month and day form a valid date; otherwise <c>false</c>.</returns>
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

    /// <summary>
    /// Validates the specified hour and throws an exception if it is not within the valid range of 0 to 24.
    /// </summary>
    /// <param name="hour">The hour to validate.</param>
    /// <param name="label">The label to include in the exception message.</param>
    private void ValidateHour(int hour, string label)
    {
        if (hour < 0 || hour > 24)
        {
            throw new ArgumentException($"Invalid {label} hour. {label} hour must be between 0 and 24.");
        }
    }

    /// <summary>
    /// Validates the specified minute and throws an exception if it is not within the valid range of 0 to 59.
    /// </summary>
    /// <param name="minute">The minute to validate.</param>
    /// <param name="label">The label to include in the exception message.</param>
    private void ValidateMinute(int minute, string label)
    {
        if (minute < 0 || minute > 59)
        {
            throw new ArgumentException($"Invalid {label} minute. {label} minute must be between 0 and 59.");
        }
    }

    /// <summary>
    /// Validates the start time and end time of a workday. Throws an exception if the start time is later than or same as the end time.
    /// </summary>
    /// <param name="startTime">The start time of the workday.</param>
    /// <param name="endTime">The end time of the workday.</param>
    private void ValidateStartTimeToEndTime(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new InvalidOperationException("Start time must be earlier than end time.");
        }
    }

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
    /// Adjusts the start date for an increment based on the specified parameters.
    /// </summary>
    /// <param name="startDate">The start date to adjust.</param>
    /// <param name="isPositiveIncrement">A boolean value indicating whether the increment is positive or negative.</param>
    /// <param name="includeDifference">A boolean value indicating whether to include the time difference in the adjustment.</param>
    /// <returns>The adjusted start date.</returns>
    private DateTime AdjustStartDateBasedOnWorkHours(DateTime startDate, bool isPositiveIncrement, bool includeDifference = false)
    {
        TimeSpan difference = isPositiveIncrement ? startDate.TimeOfDay - EndTime : startDate.TimeOfDay - StartTime;
        if (startDate.TimeOfDay < StartTime)
        {
            // If isPositiveIncrement is true, set startDate time as workday's StartTime.
            // Otherwise, subtract one day from startDate and set time as workday's EndTime.
            startDate = isPositiveIncrement ? startDate.Date.Add(StartTime) : startDate.Date.Add(EndTime).AddDays(-1);
            startDate = includeDifference ? startDate.Subtract(difference) : startDate;
        }

        if (startDate.TimeOfDay > EndTime)
        {
            // If isPositiveIncrement is true, add one day to startDate and set time as workday's StartTime.
            // Otherwise, set startDate time as workday's EndTime.
            startDate = isPositiveIncrement ? startDate.Date.Add(StartTime).AddDays(1) : startDate.Date.Add(EndTime);
            startDate = includeDifference ? startDate.Add(difference) : startDate;
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