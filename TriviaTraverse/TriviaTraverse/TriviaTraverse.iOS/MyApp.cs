using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using HealthKit;

namespace TriviaTraverse.iOS
{
    public static class MyApp
    {
        //public MyApp() { }

        public static bool HealthPermission { get; set; }

        public static HKHealthStore HealthStore { get; set; }

        public static bool HKComplete { get; set; }

        public static void ValidateAuthorization()
        {
            var stepCountType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);
            var typesToShare = new NSSet();
            var typesToRead = new NSSet(new[] { stepCountType });
            
            var access = MyApp.HealthStore.GetAuthorizationStatus(stepCountType);
            if (access.HasFlag(HKAuthorizationStatus.SharingAuthorized))
            {
                MyApp.HealthPermission = true;
                HKComplete = true;
            }
            else
            {
                HKComplete = false;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    MyApp.HealthStore.RequestAuthorizationToShare(
                                typesToShare,
                                typesToRead,
                                ReactToHealthCarePermissions);
                });
            }
        }

        public static void ReactToHealthCarePermissions(bool success, NSError error)
        {
            var stepCountType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);
            var access = MyApp.HealthStore.GetAuthorizationStatus(stepCountType);
            if (access.HasFlag(HKAuthorizationStatus.SharingAuthorized))
            {
                MyApp.HealthPermission = true;
            }
            else
            {
                MyApp.HealthPermission = false;
            }
            HKComplete = true;
        }


    }
}
