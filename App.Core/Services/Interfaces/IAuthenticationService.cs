using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public bool isAuthenticated();
        public User currentUser();
       
    }
}
