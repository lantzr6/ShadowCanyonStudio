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
using TriviaTraverse.Droid;
using System.Threading.Tasks;
using TriviaTraverse.Models;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceStepsImplementation))]
namespace TriviaTraverse.Droid
{
    class DeviceStepsImplementation : Application//, IDeviceSteps
    {

    //    public DeviceStepsImplementation() { }

    //    public void GetCumlativeSteps(List<StepData> StepQueries, DateTime EndStepTime)
    //    {
    //        //return await Task.Run(() =>
    //        //{
    //        //    int steps = ((GlobalApp)Application.Context).iCumlativeStepCount;
    //        //    return steps;
    //        //});

    //        // Get the MainActivity instance
    //        MainActivity activity = Xamarin.Forms.Forms.Context as MainActivity;

    //        //activity.getSteps(StepQueries, EndStepTime);

    //    }

    //    public async Task<bool> AskHealthPermissionAsync()
    //    {
    //        return await Task.Run(() =>
    //        {
    //          return true;
    //        });
    //    }
    }
}