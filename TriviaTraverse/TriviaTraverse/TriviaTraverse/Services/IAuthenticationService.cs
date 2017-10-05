using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaTraverse.Services
{
    public interface IAuthenticationService
    {
        bool Login(string email, string password);

        void UpdateAccountAsync();

        void Logout();
    }
}
