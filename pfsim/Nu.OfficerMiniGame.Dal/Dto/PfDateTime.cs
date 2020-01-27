using Newtonsoft.Json;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    [JsonConverter(typeof(PfDateTimeJsonConverter))]
    public class PfDateTime
    {
        private static Dictionary<int, Months> months = new Dictionary<int, Months>
        {
            {1, Months.Abadius },
            {2, Months.Calistril } ,
            {3, Months.Pharast },
            {4, Months.Gozran },
            {5, Months.Desnus },
            {6, Months.Sarenith },
            {7, Months.Erastus },
            {8, Months.Arodus },
            {9, Months.Rova },
            {10, Months.Lamashan },
            {11, Months.Neth },
            {12, Months.Kuthona }
        };

        public Months NameOfMonth { get => months[underlyingTime.Month]; }

        public DaysOfTheWeek NameOfDay { get => (DaysOfTheWeek)underlyingTime.DayOfWeek; }

        public Season Season
        {
            get
            {
                switch (NameOfMonth)
                {
                    case Months.Abadius:
                    case Months.Calistril:
                    case Months.Kuthona:
                        return Season.Winter;
                    case Months.Pharast:
                    case Months.Gozran:
                    case Months.Desnus:
                        return Season.Spring;
                    case Months.Sarenith:
                    case Months.Erastus:
                    case Months.Arodus:
                        return Season.Summer;
                    case Months.Rova:
                    case Months.Lamashan:
                    case Months.Neth:
                        return Season.Fall;
                }
                throw new NotImplementedException();
            }
        }

        private DateTime underlyingTime;

        public int Year
        {
            get => underlyingTime.Year;
        }

        public int Month
        {
            get => underlyingTime.Month;
        }

        public int Day
        {
            get => underlyingTime.Day;
        }

        public int Hour
        {
            get => underlyingTime.Hour;
        }

        public int Minute
        {
            get => underlyingTime.Minute;
        }

        public int Second
        {
            get => underlyingTime.Second;
        }

        private PfDateTime(DateTime time)
        {
            underlyingTime = time;
        }

        public PfDateTime(int year, int month, int day)
        {
            underlyingTime = new DateTime(year, month, day);
        }

        public PfDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            underlyingTime = new DateTime(year, month, day, hour, minute, second);
        }

        public string ToSemanticString()
        {
            return $"{NameOfDay}, the {underlyingTime.Day} of {NameOfMonth}, {underlyingTime.Year}";
        }

        public static TimeSpan operator -(PfDateTime left, PfDateTime right)
        {
            return left.underlyingTime - right.underlyingTime;
        }

        public static PfDateTime operator -(PfDateTime left, TimeSpan right)
        {
            return new PfDateTime(left.underlyingTime - right);
        }
        public static PfDateTime operator +(PfDateTime left, TimeSpan right)
        {
            return new PfDateTime(left.underlyingTime + right);
        }
    }
}
