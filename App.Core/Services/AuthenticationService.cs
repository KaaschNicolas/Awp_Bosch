using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;

namespace App.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private bool _isAuthenticated = false;
        private User _currentUser;

        private BoschContext _boschContext;
        public AuthenticationService(BoschContext boschContext) {
            _boschContext = boschContext;
        }
        private void authenticate() {
            var adUsername = Environment.UserName;
            var result = _boschContext.Users.Where(u => u.AdUsername.Equals(adUsername)).ToList();
            if (result.Count != 0 && result[0] != null) { 
                _isAuthenticated = true;
                _currentUser = result[0];
            }
        }

        public bool isAuthenticated()
        {
            if (_currentUser == null)
            {
               authenticate();
            }
            return _isAuthenticated;
        }

        public User currentUser()
        {
            if (_currentUser == null)
            {
               authenticate();
            }
            return _currentUser;
        }
    }
}
