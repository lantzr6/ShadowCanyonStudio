using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android.Support.V7.App;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness.Request;
using Android.Gms.Common;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Result;
using Java.Util.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;
using TriviaTraverse.Models;
using TriviaTraverse.Views;

namespace TriviaTraverse.Droid
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //public const string TAG = "TriviaTraverse";
        //const int REQUEST_OAUTH = 1;
        //const string AUTH_PENDING = "auth_state_pending";
        //const string DATE_FORMAT = "yyyy.MM.dd HH:mm:ss";
        //bool authInProgress = false;
        //GoogleApiClient mClient;
        //List<StepData> mStepQueries = null;
        //DateTime mEndStepTime = DateTime.Today;

        //int iCumlativeStepCount = 0;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);        

            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            LoadApplication(new App());

            //if (bundle != null)
            //{
            //    authInProgress = bundle.GetBoolean(AUTH_PENDING);
            //}

            
        }


        //private void BuildFitnessClient()
        //{
        //    var clientConnectionCallback = new ClientConnectionCallback();
        //    clientConnectionCallback.OnConnectedImpl = async () =>
        //    {
        //        foreach (StepData stepQ in mStepQueries)
        //        {
        //            DataReadRequest readRequest = QueryFitnessData(stepQ.StartTime, mEndStepTime);
        //            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mClient, readRequest);
        //            stepQ.Steps = GetData(dataReadResult);
        //        }

        //        var mp = App.Current.MainPage;
        //        List<int> steps = new List<int>();
        //        steps.Add(iCumlativeStepCount);
        //        steps.Add(iCumlativeStepCount+10);
        //        steps.Add(iCumlativeStepCount+100);
        //        MessagingCenter.Send<MasterPage, List<StepData>>((MasterPage)mp, "Steps", mStepQueries);
        //        mClient.Disconnect();
        //    };
        //    // Create the Google API Client
        //    mClient = new GoogleApiClient.Builder(this)
        //        .AddApi(FitnessClass.HISTORY_API)
        //        .AddScope(new Scope(Scopes.FitnessActivityRead))
        //        .AddConnectionCallbacks(clientConnectionCallback)
        //        .AddOnConnectionFailedListener(result => {
        //            Console.WriteLine("Connection failed. Cause: " + result);
        //            if (!result.HasResolution)
        //            {
        //                // Show the localized error dialog
        //                //GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, this, 0).Show();
        //                return;
        //            }
        //            // The failure has a resolution. Resolve it.
        //            // Called typically when the app is not yet authorized, and an
        //            // authorization dialog is displayed to the user.
        //            if (!authInProgress)
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Attempting to resolve failed connection");
        //                    authInProgress = true;
        //                    result.StartResolutionForResult(this, REQUEST_OAUTH);
        //                }
        //                catch (IntentSender.SendIntentException e)
        //                {
        //                    Console.WriteLine("Exception while starting resolution activity: " + e.Message);
        //                }
        //            }
        //        }).Build();

        //}

        //DataReadRequest QueryFitnessData(DateTime startTime, DateTime endTime)
        //{
        //    // Setting a start and end date using a range of 1 week before this moment.
        //    long startTimeElapsed = GetMsSinceEpochAsLong(startTime);
        //    long endTimeElapsed = GetMsSinceEpochAsLong(endTime);

        //    Console.WriteLine("Range Start: " + startTime.ToString(DATE_FORMAT));
        //    Console.WriteLine("Range End: " + endTime.ToString(DATE_FORMAT));

        //    DataSource ESTIMATED_STEP_DELTAS = new DataSource.Builder()
        //        .SetDataType(DataType.TypeStepCountDelta)
        //        .SetType(DataSource.TypeDerived)
        //        .SetStreamName("estimated_steps")
        //        .SetAppPackageName("com.google.android.gms")
        //        .Build();

        //    DataReadRequest readRequest = new DataReadRequest.Builder()
        //        //.Aggregate(ESTIMATED_STEP_DELTAS, DataType.AggregateStepCountDelta)
        //        //.Aggregate(DataType.TypeStepCountDelta, DataType.AggregateStepCountDelta)
        //        .Read(ESTIMATED_STEP_DELTAS)
        //        //.BucketByTime(1, TimeUnit.Days)
        //        .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
        //        .Build();

        //    return readRequest;
        //}

        //long GetMsSinceEpochAsLong(DateTime dateTime)
        //{
        //    return (long)dateTime.ToUniversalTime()
        //        .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
        //        .TotalMilliseconds;
        //}
        //private int GetData(DataReadResult dataReadResult)
        //{
        //    int cnt = 0;
        //    if (dataReadResult.Buckets.Count > 0)
        //    {
        //        Console.WriteLine("Number of returned buckets of DataSets is: "
        //        + dataReadResult.Buckets.Count);

        //        foreach (Bucket bucket in dataReadResult.Buckets)
        //        {
        //            IList<DataSet> dataSets = bucket.DataSets;
        //            foreach (DataSet dataSet in dataSets)
        //            {
        //                cnt += DumpDataSet(dataSet);
        //            }
        //        }
        //    }
        //    else if (dataReadResult.DataSets.Count > 0)
        //    {
        //        Console.WriteLine("Number of returned DataSets is: "
        //        + dataReadResult.DataSets.Count);

        //        foreach (DataSet dataSet in dataReadResult.DataSets)
        //        {
        //            cnt += DumpDataSet(dataSet);
        //        }
        //    }
        //    return cnt;
        //}
        
        //private int DumpDataSet(DataSet dataSet)
        //{
        //    Console.WriteLine("Data returned for Data type: " + dataSet.DataType.Name);
        //    int cnt = 0;
        //    foreach (DataPoint dp in dataSet.DataPoints)
        //    {
        //        if (dp.OriginalDataSource.StreamName != "user_input")
        //        {
        //            //Console.WriteLine("Data point:");
        //            //Console.WriteLine("\tType: " + dp.DataType.Name);
        //            //Console.WriteLine("\tStart: " + new DateTime(1970, 1, 1).AddMilliseconds(dp.GetStartTime(TimeUnit.Milliseconds)).ToString(DATE_FORMAT));
        //            //Console.WriteLine("\tEnd: " + new DateTime(1970, 1, 1).AddMilliseconds(dp.GetEndTime(TimeUnit.Milliseconds)).ToString(DATE_FORMAT));
        //            foreach (Field field in dp.DataType.Fields)
        //            {
        //                Console.WriteLine("\tField: " + field.Name +
        //                " Value: " + dp.GetValue(field));
        //                cnt += dp.GetValue(field).AsInt();
        //            }
        //        }
        //    }
        //    return cnt;
        //}

        //protected override void OnStart()
        //{
        //    base.OnStart();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //    if (mClient != null && mClient.IsConnected)
        //    {
        //        mClient.Disconnect();
        //    }
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //}

        //protected override void OnSaveInstanceState(Bundle outState)
        //{
        //    base.OnSaveInstanceState(outState);
        //}

        //class ClientConnectionCallback : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks
        //{
        //    public Action OnConnectedImpl { get; set; }

        //    public void OnConnected(Bundle connectionHint)
        //    {
        //        Console.WriteLine("Connected!!!");

        //        OnConnectedImpl();
        //    }

        //    public void OnConnectionSuspended(int cause)
        //    {
        //        if (cause == GoogleApiClient.ConnectionCallbacks.CauseNetworkLost)
        //        {
        //            Console.WriteLine("Connection lost.  Cause: Network Lost.");
        //        }
        //        else if (cause == GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected)
        //        {
        //            Console.WriteLine("Connection lost.  Reason: Service Disconnected");
        //        }
        //    }
        //}

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    if (requestCode == REQUEST_OAUTH)
        //    {
        //        authInProgress = false;
        //        if (resultCode == Result.Ok)
        //        {
        //            if (mClient != null && !mClient.IsConnecting && !mClient.IsConnected)
        //            {
        //                mClient.Connect();
        //            }
        //        }
        //        else if (resultCode == Result.Canceled)
        //        {
        //            Console.WriteLine("GoogleFit: RESULT_CANCELED");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("GoogleFit: requestCode NOT request_oauth");
        //    }
        //}

        //public void getSteps(List<StepData> StepQueries, DateTime EndStepTime)
        //{
        //    mStepQueries = StepQueries;
        //    mEndStepTime = EndStepTime;
        //    if (mClient == null) { BuildFitnessClient(); }
        //    if (!mClient.IsConnecting && !mClient.IsConnected)
        //    {
        //        mClient.Connect();
        //    }
        //}

    }

}

