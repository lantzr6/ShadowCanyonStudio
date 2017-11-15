using System;

namespace TriviaTraverse.Facebook.Models
{
    public class FacebookProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AppId { get; set; }

        public FacebookProfile(string fname, string lname, string name, string email, string appId)
        {
            FirstName = fname;
            LastName = lname;
            Name = name;
            Email = email;
            AppId = appId;
        }
    }
}

