using System;

namespace TriviaTraverse.Facebook.Models
{
    public class FacebookFriend
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public FacebookFriend(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("[FacebookFriend: Name={0}, Id={1}]", Name, Id);
        }
    }
}