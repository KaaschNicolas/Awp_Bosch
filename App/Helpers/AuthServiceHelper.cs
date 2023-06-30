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
        public static Role Rolle;
        public static bool IsAdmin()
        {
            return hasRole(Role.Admin);
        }

        public static bool IsLesezugriff()
        {
                return hasRole(Role.Lesezugriff);
        }

        public static bool IsStandardUser()
        {
            return hasRole(Role.StandardUser);
        }

        public static bool IsStandardUserOrAdmin()
        {
            return hasRole(Role.StandardUser) || hasRole(Role.Admin);
        }

        public static Visibility IsVisibleAdmin()
        {
            if (IsAdmin())
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public static Visibility IsVisible()
        {
            if (IsAdmin() || IsStandardUser())
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public static bool hasRole(Role role)
        {
            return Rolle.Equals(role);
        }

    }
}
