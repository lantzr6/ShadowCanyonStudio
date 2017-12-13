using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TriviaTraverse.iOS.Helpers
{
    static class NSDateHelper
    {
        public static NSDate ConvertDateTimeToNSDate(DateTime date)
        {
            DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - newDate).TotalSeconds);
        }
        public static DateTime ConvertNSDateToDateTime(NSDate date)
        {
            DateTime refDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            DateTime retVal = refDate;
            if (date != null)
            {
                retVal = refDate.AddSeconds(date.SecondsSinceReferenceDate);
            }
            return retVal;
        }
    }
}