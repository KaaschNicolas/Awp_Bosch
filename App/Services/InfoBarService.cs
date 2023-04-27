using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Services
{
 
    public class InfoBarService : IInfoBarService
    {
        private InfoBar? _infoBar;
        public InfoBar? InfoBar
        {
            get
            {
                if (_infoBar == null)
                {
                    _infoBar = new InfoBar();
                }
                return _infoBar;

            }
            set
            {
                _infoBar = value;
            }
        }

   

        public void showMessage(string message, string title, InfoBarSeverity severity=InfoBarSeverity.Success)
        {
        
        if (_infoBar != null)
            {
                _infoBar.Title = title;
                _infoBar.Message = message;
                _infoBar.IsOpen = true;
                _infoBar.Severity = severity;
                _infoBar.VerticalAlignment = VerticalAlignment.Bottom;
            }
        }

        public void showError(string message, string title, InfoBarSeverity severity=InfoBarSeverity.Error)
        {
            showMessage(message, title, severity);
        }


        public void showError(Exception exception, string title, InfoBarSeverity severity = InfoBarSeverity.Error)
        {
            showMessage(exception.Message, title, severity);
        }
    }
}
