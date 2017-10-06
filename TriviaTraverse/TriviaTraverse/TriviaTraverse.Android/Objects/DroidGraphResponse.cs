using System;
using TriviaTraverse.Droid.Objects;
using TriviaTraverse.Facebook.Objects;
using Xamarin.Facebook;

[assembly: Xamarin.Forms.Dependency(typeof(DroidGraphResponse))]
namespace TriviaTraverse.Droid.Objects
{
    public class DroidGraphResponse : IGraphResponse
    {
        public string RawResponse { get; set; }

        public DroidGraphResponse(GraphResponse graphResponse)
        {
            RawResponse = graphResponse.RawResponse;
        }
    }
}

