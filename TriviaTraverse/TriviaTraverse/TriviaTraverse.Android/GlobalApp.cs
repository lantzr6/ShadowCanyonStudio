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

namespace TriviaTraverse.Droid
{
    [Application]
    public class GlobalApp : Application
    {
        public int iCumlativeStepCount = 0;
        public GlobalApp(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        { }
        public override void OnCreate()
        {
            iCumlativeStepCount = 0;
            base.OnCreate();
        }
    }
}