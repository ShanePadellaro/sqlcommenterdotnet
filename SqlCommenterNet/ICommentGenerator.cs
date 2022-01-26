using System.Collections.Generic;
using System.Data.Common;

namespace SqlCommenter
{
    public interface ICommentGenerator
    {
        string GenerateSqlComment(Dictionary<string,string> attributes);
        DbCommand ManipulateCommand(DbCommand command, string comment);
    }
}