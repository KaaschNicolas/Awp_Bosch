using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class CommentValidator
{
    private Comment _comment;

    public CommentValidator(Comment comment)
    {
        _comment = comment;
    }

    private List<string> Validate()
    {
        List<string> errors = new List<string>();

        if(string.IsNullOrEmpty(_comment.Name) || _comment.Name.Length > 650)
        {
            errors.Add("Name darf nicht null sein oder 650 Zeichen überschreiten.");
        }

        if(_comment.NotedBy == null)
        {
            errors.Add("NotedBy darf nicht null sein.");
        }

        return errors;
    }

    public bool IsValid()
    {
        if (Validate().Count == 0)
        {
            return true;
        }

        return false;
    }

    public List<string> GetErrors()
    {
        return Validate();
    }
}
