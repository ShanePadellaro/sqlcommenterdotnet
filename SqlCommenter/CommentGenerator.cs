using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Encodings.Web;

namespace SqlCommenter
{
    public class CommentGenerator:ICommentGenerator
    {
        public string GenerateSqlComment(Dictionary<string, string> attributes)
        {
            var dict = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var key = UrlEncoder.Default.Encode(attribute.Key);
                key = MetaEscape(key);
                var value = UrlEncoder.Default.Encode(attribute.Value);
                value = MetaEscape(value);
                value = $"'{value}'";
                dict.Add(key, value);
            }

            var ordered = dict.OrderByDescending(x => x.Key);

            string comment = "";
            foreach (var keyValuePair in ordered)
            {
                comment += $",{keyValuePair.Key}={keyValuePair.Value}";
            }

            comment = comment.Substring(1);
            return comment;
        }

        public DbCommand ManipulateCommand(DbCommand command, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment) && command != null)
                command.CommandText += "/*" + comment + "*/";

            return command;
        }

        public string MetaEscape(string encodedValue)
        {
            return encodedValue.Replace("'", "/'");
        }
    }
}