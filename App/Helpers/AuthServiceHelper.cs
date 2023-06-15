using App.Core.DataAccess;
using App.Core.Helpers;
using App.Core.Models.Enums;
using App.Core.Services;
using App.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace App.Helpers
{
    public static class AuthServiceHelper
    {
        public static void Init(BoschContext boschContext)
        {
            _authenticationService = new AuthenticationService(boschContext);
        }

        private static IAuthenticationService _authenticationService;

        public static bool IsAdmin()
        {
            var ts = Rolle;
            if (_authenticationService != null)
            {
                return _authenticationService.IsAdmin;
            }
            return false;
        }

        public static Role Rolle;

        public static bool IsLesezugriff()
        {
            if (_authenticationService != null)
            {
                return _authenticationService.IsLesezugriff;

            }
            return false;
        }

        public static bool IsSchichtleitung()
        {
            if (_authenticationService != null)
            {
                return _authenticationService.IsSchichtleitung;

            }
            return false;
        }

        public static Visibility IsVisible()
        {
            if (IsAdmin())
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }


    }
}
