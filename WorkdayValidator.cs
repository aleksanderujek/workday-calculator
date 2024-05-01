class WorkdayValidator
{
    /// <summary>
    /// Validates if provided <see cref="TimeSpan"/> values are not empty.
    /// </summary>
    /// <param name="startTime">Workday Start Time <see cref="TimeSpan"/> to verify.</param>
    /// <param name="startTime">Workday End Time <see cref="TimeSpan"/> to verify.</param>
    public void ValidateTimeSpanIfEmpty(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime == TimeSpan.Zero || endTime == TimeSpan.Zero)
        {
            throw new InvalidOperationException("Workday start and stop times must be set before calculating workday increment.");
        }
    }

    /// <summary>
    /// Validates if provided <see cref="DateTime"/> value is not empty.
    /// </summary>
    /// <param name="date">Workday <see cref="DateTime"/> to verify.</param>
    public void ValidateDateTimeIfEmpty(DateTime date)
    {
        if (date == DateTime.MinValue)
        {
            throw new ArgumentException($"DateTime cannot be empty.");
        }
    }

    /// <summary>
    /// Checks if the specified month and day form a valid date.
    /// </summary>
    /// <param name="month">The month component of the date.</param>
    /// <param name="day">The day component of the date.</param>
    /// <returns><c>true</c> if the specified month and day form a valid date; otherwise <c>false</c>.</returns>
    public void ValidateDate(int month, int day)
    {
        try
        {
            DateTime date = new DateTime(DateTime.Now.Year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ArgumentException("Invalid date. The specified date does not exist.");
        }
    }

    /// <summary>
    /// Validates the start time and end time of a workday. Throws an exception if the start time is later than or same as the end time.
    /// </summary>
    /// <param name="startTime">The start time of the workday.</param>
    /// <param name="endTime">The end time of the workday.</param>
    public void ValidateStartTimeToEndTime(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new InvalidOperationException("Start time must be earlier than end time.");
        }
    }

    /// <summary>
    /// Validates the specified minute and throws an exception if it is not within the valid range of 0 to 59.
    /// </summary>
    /// <param name="minute">The minute to validate.</param>
    /// <param name="label">The label to include in the exception message.</param>
    public void ValidateMinute(int minute, string label)
    {
        if (minute < 0 || minute > 59)
        {
            throw new ArgumentException($"Invalid {label} minute. {label} minute must be between 0 and 59.");
        }
    }

    /// <summary>
    /// Validates the specified hour and throws an exception if it is not within the valid range of 0 to 24.
    /// </summary>
    /// <param name="hour">The hour to validate.</param>
    /// <param name="label">The label to include in the exception message.</param>
    public void ValidateHour(int hour, string label)
    {
        if (hour < 0 || hour > 24)
        {
            throw new ArgumentException($"Invalid {label} hour. {label} hour must be between 0 and 24.");
        }
    }
}