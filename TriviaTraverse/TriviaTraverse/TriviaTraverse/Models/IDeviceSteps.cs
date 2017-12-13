using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TriviaTraverse.Models
{
    public interface IHealthData
    {
        //https://github.com/jlmatus/Xamarin-Forms-HealthKit-steps

        void FetchSteps(Action<List<StepData>> completionHandler, List<StepData> StepQueries);
        void GetHealthPermissionAsync(Action<bool> completion);
    }
}