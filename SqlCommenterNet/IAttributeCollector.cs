using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{
    public interface IAttributeCollector
    {
        Dictionary<string,string> GetAttributes(ActionContext context, CommandEventData eventData);
    }
}