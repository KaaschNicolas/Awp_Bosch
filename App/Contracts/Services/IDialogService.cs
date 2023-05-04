using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Contracts.Services;
public interface IDialogService
{
    //TODO Extend the Interface -> InputDialog 
    Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText="Löschen", string cancelButtonText="Abbrechen");
}
