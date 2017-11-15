using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaTraverse.Models;

namespace TriviaTraverse.Services
{
    public interface IAuthenticationService
    {
        Task<Player> LoginAsync(string email, string password);

        Task UpdateAccountAsync();

        void Logout();
    }
}
