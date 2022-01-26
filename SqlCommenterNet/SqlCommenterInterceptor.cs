using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{

    public class SqlCommenterInterceptor : DbCommandInterceptor
    {
        private readonly IActionContextAccessor _contextAccessor;

        public SqlCommenterInterceptor(IActionContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);

            return result;
        }

        #if NET5_0_OR_GREATER
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return new ValueTask<InterceptionResult<DbDataReader>>(Task.FromResult(result));
        }
        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<object> result)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<int> result)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);

            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }
        #else
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return Task.FromResult(result);
        }
    

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<object> result)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<int> result)
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            AddSqlComment(_contextAccessor?.ActionContext, command, eventData);

            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }
#endif


        private void AddSqlComment(ActionContext context, DbCommand command, CommandEventData commandEventData)
        {
            if (command == null) return;

            var attributes = new Dictionary<string, string>();
            attributes.Add("framework", $"ASP.NET");

            if (context != null)
            {
                var rd = context.RouteData;
                if (context?.HttpContext?.Request?.Path != null)
                    attributes.Add("route", context?.HttpContext?.Request?.Path);

                if (rd.Values.TryGetValue("controller", out var controller))
                    attributes.Add("controller", controller.ToString());

                if (rd.Values.TryGetValue("action", out var action))
                    attributes.Add("action", action.ToString());

                var headers = context.HttpContext.Request.Headers;
                if (headers.TryGetValue("traceparent", out var traceParent))
                    attributes.Add("traceparent", traceParent.ToString());
                if (headers.TryGetValue("tracestate", out var tracestate))
                    attributes.Add("tracestate", traceParent.ToString());
            }


            if (commandEventData?.Context?.Database?.ProviderName != null)
                attributes.Add("db_driver", commandEventData.Context.Database.ProviderName);


            var comment = GenerateComment(attributes);
            ManipulateCommand(command, comment);

        }

        private void ManipulateCommand(DbCommand command, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment) && command != null)
                command.CommandText += "/*" + comment + "*/";
        }

        private string GenerateComment(Dictionary<string, string> attributes)
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

        private string MetaEscape(string encodedValue)
        {
            return encodedValue.Replace("'", "/'");
        }
    }
}