using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public bool IsAuthenticated { get; }
        public User CurrentUser { get; }
        public bool IsDbActive { get; }
        public bool IsAdmin { get; }
        public bool IsLesezugriff { get; }
        public bool IsSchichtleitung { get; }

    }
}
