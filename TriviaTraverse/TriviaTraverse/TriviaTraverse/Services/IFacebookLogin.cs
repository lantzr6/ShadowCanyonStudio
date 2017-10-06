using System;
using System.Threading.Tasks;
using TriviaTraverse.Facebook.Objects;

namespace TriviaTraverse.Facebook.Services
{
    public interface IFacebookLogin
    {

        Task<FbAccessToken> LogIn(string[] permissions);
        bool IsLoggedIn();
        FbAccessToken GetAccessToken();
        void Logout();
    }
}
