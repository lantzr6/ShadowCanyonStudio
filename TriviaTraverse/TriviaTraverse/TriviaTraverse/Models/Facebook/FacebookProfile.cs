using System;

namespace TriviaTraverse.Facebook.Models
{
    public class FacebookProfile
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AppId { get; set; }

        public FacebookProfile(string name, string email, string appId)
        {
            Name = name;
            Email = email;
            AppId = appId;
        }
    }
}

