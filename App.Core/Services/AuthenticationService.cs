using App.Core.Models;
using App.Core.Services.Interfaces;

namespace App.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private bool _isAuthenticated;
        private User _currentUser;

        private ICrudService<User> _crudService;
        public AuthenticationService(ICrudService<User> crudService) {
            _crudService = crudService;
            authenticateUser();
        }

        public async void authenticateUser() {
            _isAuthenticated = false;
            var adUsername = Environment.UserName;
            var result = await _crudService.GetAll();
            var users = result.Data;
            foreach (var user in users)
            {
                _isAuthenticated = String.Equals(adUsername, user.AdUsername);
                if (_isAuthenticated) {
                    _currentUser = user;
                    break;
                }
            }
        }
        public bool isAuthenticated()
        {
            return _isAuthenticated;
        }

        public User currentUser()
        {
            return _currentUser;
        }
    }
}
