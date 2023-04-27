using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace App.Contracts.Services
{
    public interface IInfoBarService
    {
        InfoBar? InfoBar
        {
            get; set;
        } 
        void showMessage(string message, string title, InfoBarSeverity severity=InfoBarSeverity.Success);
        void showError(string errorMessage, string title, InfoBarSeverity severity=InfoBarSeverity.Error);
        void showError(Exception exception, string title, InfoBarSeverity severity=InfoBarSeverity.Error);
    }

}
