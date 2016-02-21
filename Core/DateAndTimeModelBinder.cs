using System;
using System.Web.Mvc;

namespace GovEventer.Core
{
    public class DateTimeViewModel
    {
        public DateTime? DateTime { get; set; }

        public DateTimeViewModel()
        {

        }

        public DateTimeViewModel(DateTime? dateTime)
        {
            DateTime = dateTime;
            if (dateTime != null)
            {
                var dt = dateTime.Value;
                Year = dt.Year;
                Month = dt.Month;
                Day = dt.Day;
                Hour = dt.Hour;
                Minute = dt.Minute;
                Second = dt.Second;
            }
        }

        public DateTimeViewModel(int? year, int? month, int? day, int? hour, int? minute, int? second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? Second { get; set; }

        public DateTime? ToDateTime()
        {
            if (DateTime != null) return DateTime;
            return Year != null && Month != null && Day != null
                ? (DateTime?) (Hour != null && Minute != null && Second != null
                    ? new DateTime(Year.Value, Month.Value, Day.Value, Hour.Value, Minute.Value, Second.Value)
                    : new DateTime(Year.Value, Month.Value, Day.Value))
                : null;
        }
    }
    public class DateAndTimeModelBinder : IModelBinder
    {
        public string Date { get; set; }
        public string Time { get; set; }

        public string Month { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }

        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public bool DateSet
        {
            get { return !string.IsNullOrEmpty(Date); }
        }

        public bool MonthDayYearSet
        {
            get { return !(string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Year)); }
        }

        public bool TimeSet
        {
            get { return !string.IsNullOrEmpty(Time); }
        }

        public bool HourMinuteSecondSet
        {
            get
            {
                return !(string.IsNullOrEmpty(Hour) && string.IsNullOrEmpty(Minute) && string.IsNullOrEmpty(Second));
            }
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            //Maybe we're lucky and they just want a DateTime the regular way.
            DateTime? dateTimeAttempt = GetA<DateTime>(bindingContext, "DateTime");
            if (dateTimeAttempt != null)
            {
                return dateTimeAttempt.Value;
            }

            //If they haven't set Month,Day,Year OR Date, set "date" and get ready for an attempt
            if (MonthDayYearSet == false && DateSet == false)
            {
                Date = "Date";
            }

            //If they haven't set Hour, Minute, Second OR Time, set "time" and get ready for an attempt
            if (HourMinuteSecondSet == false && TimeSet == false)
            {
                Time = "Time";
            }

            //Did they want the Date *and* Time?
            DateTime? dateAttempt = GetA<DateTime>(bindingContext, Date);
            DateTime? timeAttempt = GetA<DateTime>(bindingContext, Time);

            //Maybe they wanted the Time via parts
            if (HourMinuteSecondSet)
            {
                var hour = GetA<int>(bindingContext, Hour).GetValueOrDefault(0);
                var minute = GetA<int>(bindingContext, Minute).GetValueOrDefault(0);
                var second = GetA<int>(bindingContext, Second).GetValueOrDefault(0);
                if (hour == -1 && minute == -1 && second == -1)
                    timeAttempt = null;
                else
                    timeAttempt = new DateTime(
                        DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                        hour,
                        minute,
                        second);
            }

            //Maybe they wanted the Date via parts
            if (MonthDayYearSet)
            {
                var year = GetA<int>(bindingContext, Year).GetValueOrDefault(0);
                var month = GetA<int>(bindingContext, Month).GetValueOrDefault(0);
                var day = GetA<int>(bindingContext, Day).GetValueOrDefault(0);
                if (year == 0 && month == 0 && day == 0)
                    dateAttempt = null;
                else
                {
                    var daysInMonth = DateTime.DaysInMonth(year, month);
                    dateAttempt = new DateTime(
                            year,
                            month,
                            day > daysInMonth ? daysInMonth : day,
                            0, 0, 0);
                }

            }

            //If we got both parts, assemble them!
            if (dateAttempt != null && timeAttempt != null)
            {
                return new DateTime(dateAttempt.Value.Year,
                    dateAttempt.Value.Month,
                    dateAttempt.Value.Day,
                    timeAttempt.Value.Hour,
                    timeAttempt.Value.Minute,
                    timeAttempt.Value.Second);
            }
            //Only got one half? Return as much as we have!
            return dateAttempt ?? timeAttempt;
        }

        private T? GetA<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (string.IsNullOrEmpty(key)) return null;
            //Try it with the prefix...
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            if (valueResult == null)
            {
                return null;
            }
            return (T?)valueResult.ConvertTo(typeof(T));
        }

    }

    public class DateAndTimeAttribute : CustomModelBinderAttribute
    {
        private readonly IModelBinder _binder;

        public DateAndTimeAttribute()
        {
            _binder = new DateAndTimeModelBinder()
            {
                Date = "Date",
                Time = "Time",
                Day = "Day",
                Month = "Month",
                Year = "Year",
                Hour = "Hour",
                Minute = "Minute",
                Second = "Second"
            };
        }
        // The user cares about a full date structure and full
        // time structure, or one or the other.
        public DateAndTimeAttribute(string date, string time)
        {
            _binder = new DateAndTimeModelBinder
            {
                Date = date,
                Time = time
            };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string year, string month, string day)
        {
            _binder = new DateAndTimeModelBinder
            {
                Day = day,
                Month = month,
                Year = year
            };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string year, string month, string day,
            string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
            {
                Day = day,
                Month = month,
                Year = year,
                Hour = hour,
                Minute = minute,
                Second = second
            };
        }

        // The user wants to capture the date and time (or only one)
        // as individual portions.
        public DateAndTimeAttribute(string date, string time,
            string year, string month, string day,
            string hour, string minute, string second)
        {
            _binder = new DateAndTimeModelBinder
            {
                Day = day,
                Month = month,
                Year = year,
                Hour = hour,
                Minute = minute,
                Second = second,
                Date = date,
                Time = time
            };
        }

        public override IModelBinder GetBinder()
        {
            return _binder;
        }
    }

    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(DateTime) || modelType == typeof(DateTime?))
                return new DateAndTimeModelBinder()
                {
                    Date = "Date",
                    Time = "Time",
                    Day = "Day",
                    Month = "Month",
                    Year = "Year",
                    Hour = "Hour",
                    Minute = "Minute",
                    Second = "Second"
                };
            return null;
        }
    }
}