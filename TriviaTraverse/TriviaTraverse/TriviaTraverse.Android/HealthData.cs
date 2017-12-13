using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using TriviaTraverse.Droid;
using TriviaTraverse.Models;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness.Request;
using Android.Gms.Common;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Result;
using Java.Util.Concurrent;

[assembly: Dependency(typeof(HealthData))]
namespace TriviaTraverse.Droid
{
    public class HealthData : IHealthData
    {
        public const string TAG = "TriviaTraverse";
        const int REQUEST_OAUTH = 1;
        const string AUTH_PENDING = "auth_state_pending";
        const string DATE_FORMAT = "yyyy.MM.dd HH:mm:ss";
        bool authInProgress = false;
        List<StepData> mStepQueries = null;
        GoogleApiClient mClient;
        DateTime mStartStepTime = DateTime.Today;
        DateTime mEndStepTime = DateTime.Today;

        public void GetHealthPermissionAsync(Action<bool> completion)
        {
            completion(true);
        }

        public void FetchSteps(Action<List<StepData>> completionHandler, List<StepData> StepQueries)
        {
            mStepQueries = StepQueries;
            if (mClient == null)
            {
                BuildFitnessClient(completionHandler);
                mClient.Connect();
            }
            if (!mClient.IsConnecting && !mClient.IsConnected)
            {
                mClient.Connect();
            }
        }


        private void BuildFitnessClient(Action<List<StepData>> completionHandler)
        {
            var clientConnectionCallback = new ClientConnectionCallback();
            clientConnectionCallback.OnConnectedImpl = async () =>
            {
                Console.WriteLine("Android Fit Connect: " + mStartStepTime.ToString());

                foreach (StepData stepQ in mStepQueries)
                {
                    DataReadRequest readRequest = QueryFitnessData(stepQ.StartTime, DateTime.Now);
                    var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mClient, readRequest);
                    GetData(dataReadResult, stepQ);
                }

                completionHandler(mStepQueries);
                mClient.Disconnect();
            };
            // Create the Google API Client
            // Get the MainActivity instance
            MainActivity activity = Xamarin.Forms.Forms.Context as MainActivity;
            mClient = new GoogleApiClient.Builder(activity)
                .AddApi(FitnessClass.HISTORY_API)
                .AddScope(new Scope(Scopes.FitnessActivityRead))
                .AddConnectionCallbacks(clientConnectionCallback)
                .AddOnConnectionFailedListener(result => {
                    Console.WriteLine("Connection failed. Cause: " + result);
                    if (!result.HasResolution)
                    {
                        // Show the localized error dialog
                        //GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, this, 0).Show();
                        return;
                    }
                    // The failure has a resolution. Resolve it.
                    // Called typically when the app is not yet authorized, and an
                    // authorization dialog is displayed to the user.
                    if (!authInProgress)
                    {
                        try
                        {
                            Console.WriteLine("Attempting to resolve failed connection");
                            authInProgress = true;
                            result.StartResolutionForResult(activity, REQUEST_OAUTH);
                        }
                        catch (IntentSender.SendIntentException e)
                        {
                            Console.WriteLine("Exception while starting resolution activity: " + e.Message);
                        }
                    }
                }).Build();
        }

        DataReadRequest QueryFitnessData(DateTime startTime, DateTime endTime)
        {
            // Setting a start and end date using a range of 1 week before this moment.
            long startTimeElapsed = GetMsSinceEpochAsLong(startTime);
            long endTimeElapsed = GetMsSinceEpochAsLong(endTime);

            Console.WriteLine("Range Start: " + startTime.ToString(DATE_FORMAT));
            Console.WriteLine("Range End: " + endTime.ToString(DATE_FORMAT));

            DataSource ESTIMATED_STEP_DELTAS = new DataSource.Builder()
                .SetDataType(DataType.TypeStepCountDelta)
                .SetType(DataSource.TypeDerived)
                .SetStreamName("estimated_steps")
                .SetAppPackageName("com.google.android.gms")
                .Build();

            DataReadRequest readRequest = new DataReadRequest.Builder()
                //.Aggregate(ESTIMATED_STEP_DELTAS, DataType.AggregateStepCountDelta)
                //.Aggregate(DataType.TypeStepCountDelta, DataType.AggregateStepCountDelta)
                .Read(ESTIMATED_STEP_DELTAS)
                //.BucketByTime(1, TimeUnit.Days)
                .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                .Build();

            return readRequest;
        }

        long GetMsSinceEpochAsLong(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
        private void GetData(DataReadResult dataReadResult, StepData data)
        {
            if (dataReadResult.Buckets.Count > 0)
            {
                Console.WriteLine("Number of returned buckets of DataSets is: "
                + dataReadResult.Buckets.Count);

                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    IList<DataSet> dataSets = bucket.DataSets;
                    foreach (DataSet dataSet in dataSets)
                    {
                        DumpDataSet(dataSet, data);
                    }
                }
            }
            else if (dataReadResult.DataSets.Count > 0)
            {
                Console.WriteLine("Number of returned DataSets is: "
                + dataReadResult.DataSets.Count);

                foreach (DataSet dataSet in dataReadResult.DataSets)
                {
                    DumpDataSet(dataSet, data);
                }
            }
        }

        private void DumpDataSet(DataSet dataSet, StepData data)
        {
            Console.WriteLine("Data returned for Data type: " + dataSet.DataType.Name);
            //StepData data = new StepData();
            foreach (DataPoint dp in dataSet.DataPoints)
            {
                if (dp.OriginalDataSource.StreamName != "user_input")
                {
                    foreach (Field field in dp.DataType.Fields)
                    {
                        Console.WriteLine("\tEnd: " + new DateTime(1970, 1, 1).AddMilliseconds(dp.GetEndTime(TimeUnit.Milliseconds)).ToString(DATE_FORMAT));
                        Console.WriteLine("\tField: " + field.Name +
                        " Value: " + dp.GetValue(field));
                        data.Steps += dp.GetValue(field).AsInt();
                        DateTime dataEndDate = new DateTime(1970, 1, 1).AddMilliseconds(dp.GetEndTime(TimeUnit.Milliseconds)).ToLocalTime();
                        if (data.EndTime < dataEndDate) { data.EndTime = dataEndDate; }
                    }
                }
            }
            //return data;
        }

        //class StepData
        //{
        //    public DateTime endDate { get; set; }
        //    public int Steps { get; set; }

        //    public StepData()
        //    {
        //        endDate = DateTime.MinValue;
        //        Steps = 0;
        //    }
        //}

        class ClientConnectionCallback : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks
        {
            public Action OnConnectedImpl { get; set; }

            public void OnConnected(Bundle connectionHint)
            {
                Console.WriteLine("Connected!!!");

                OnConnectedImpl();
            }

            public void OnConnectionSuspended(int cause)
            {
                if (cause == GoogleApiClient.ConnectionCallbacks.CauseNetworkLost)
                {
                    Console.WriteLine("Connection lost.  Cause: Network Lost.");
                }
                else if (cause == GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected)
                {
                    Console.WriteLine("Connection lost.  Reason: Service Disconnected");
                }
            }
        }

    }
}