using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{
    public interface ISqlCommenterService
    {
        void AddSqlComment(DbCommand command, CommandEventData eventData);
    }
}