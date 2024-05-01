# UseCases and solutions

## Positive Increment

### Example 1
Start Time         `08:00`
End Time            `16:00`

Total Hours Time    `8h`

Start date          `2024-04-24 04:00`
Increment           `0.5`
Adjusted hours      `4h`

Should move to `08:00`, then add 0.5, which is 4h => `2024-04-24 12:00`

### Example 2
Start Time         `08:00`
End Time            `16:00`

Total Hours Time    `8h`

Start date          `2024-04-24 14:00`
Increment           `2.5`
Calc Increment      `2d 4h`

Should calculate used time in this day: EndTime - StartDate = `2h`
Then deduct it from Calc Increment: `2d 2h`
Then it should check if there are still remaining hours
    If yes, then add one day and switch hours to Start Time
    If no, then just add days.

In this example it will resolve to `2024-04-28 10:00`

### Example 3
Start Time         `08:00`
End Time           `20:00`

Total Hours Time   `12h`

Start date         `2024-04-24 19:00`
Increment          `0.5`
Calc Increment     `6h`

Should calculate used time in this day: EndTime - StartDate = `1h`
Then deduct it from Calc Increment `5h`
Then since it's still above 0, it should add one day and set it as `Start Time`
And then apply remaining increment: `5h` => `2024-04-25 13:00`

### Example 4
Start Time         `08:00`
End Time           `20:00`

Total Hours Time   `12h`

Start date         `2024-04-24 22:00`
Increment          `1.75`
Calc Increment     `1d 9h`

Since startDate is after end time, we should move to next day and set time as StartTime.
Then we should add hours: `2024-04-25 17:00`
And then we should add days: `2024-04-26 17:00`.


## Negative Increment
### Example 1
Start Time         `08:00`
End Time           `16:00`

Total Hours Time   `8h`

Start date         `2024-04-24 9:00`
Increment          `-0.5`
Calc Increment     `4h`

Should calculate used time in this day: StartDate - StartTime = `1h`
Then it should substract it from increment: `3h`
Then if there is still some calc increment, it should move to day before and set time as EndTime.
From there it should substract `3h` making it: `2024-04-23 13:00`

### Example 2
24-05-2004 18:05 with an addition of -5.5 work days is 14-05-2004 12:00

Start Time         `08:00`
End Time           `16:00`

Total Hours Time   `8h`

Start date         `2024-04-24 18:05`
Increment          `-5.5`
Calc Increment     `-5d 4h`

Should move the hours to Endtime.
Then it should 

