using System;
using Xamarin.Forms;
using Foundation;
using HealthKit;
using System.Threading.Tasks;
using TriviaTraverse.Models;
using TriviaTraverse.iOS;
using TriviaTraverse.iOS.Helpers;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(HealthData))]
namespace TriviaTraverse.iOS
{
    public class HealthData : IHealthData
    {
        Action<List<StepData>> mCompletionHandler;
        List<StepData> mStepQueries;

        public HKHealthStore HealthStore { get; set; }

        NSSet DataTypesToWrite
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {

                });
            }
        }

        NSSet DataTypesToRead
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
                    HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount)
                });
            }
        }

        public void GetHealthPermissionAsync(Action<bool> completion)
        {
            if (HKHealthStore.IsHealthDataAvailable)
            {
                HealthStore = new HKHealthStore();
                HealthStore.RequestAuthorizationToShare(DataTypesToWrite, DataTypesToRead, (bool authorized, NSError error) =>
                {
                    completion(authorized);
                });
            }
            else
            {
                completion(false);
            }
        }

        public void FetchSteps(Action<List<StepData>> completionHandler, List<StepData> StepQueries)
        {
            //var calendar = NSCalendar.CurrentCalendar;
            mCompletionHandler = completionHandler;
            mStepQueries = StepQueries;

            var stepsQuantityType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);

            foreach (StepData item in mStepQueries)
            {
                var predicate = HKQuery.GetPredicateForSamples((NSDate)item.StartTime, (NSDate)DateTime.Now, HKQueryOptions.StrictStartDate);

                var query = new HKStatisticsQuery(stepsQuantityType, predicate, HKStatisticsOptions.CumulativeSum,
                                (HKStatisticsQuery resultQuery, HKStatistics results, NSError error) =>
                                {
                                    //if (error != null && completionHandler != null)
                                       //completionHandler(0, DateTime.MinValue);

                                    NSDate resultsEndDate = results.EndDate;

                                    var totalSteps = results.SumQuantity();
                                    if (totalSteps == null)
                                        totalSteps = HKQuantity.FromQuantity(HKUnit.Count, 0.0);

                                    item.Steps = Convert.ToInt32(totalSteps.GetDoubleValue(HKUnit.Count));
                                    item.EndTime = NSDateHelper.ConvertNSDateToDateTime(resultsEndDate);
                                    item.Complete = true;

                                    QueryCompletion();

                                    //completionHandler(Convert.ToInt32(totalSteps.GetDoubleValue(HKUnit.Count)), NSDateHelper.ConvertNSDateToDateTime(resultsEndDate));
                                });
                HealthStore.ExecuteQuery(query);
            }
        }

        public void QueryCompletion()
        {
            bool notComplete = mStepQueries.Where(o => !o.Complete).Any();

            if (!notComplete)
                mCompletionHandler(mStepQueries);
        }

    }
}