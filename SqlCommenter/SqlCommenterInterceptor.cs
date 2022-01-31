using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{

    public class SqlCommenterInterceptor : DbCommandInterceptor
    {
        private readonly ISqlCommenterService _commenterService;

        public SqlCommenterInterceptor(ISqlCommenterService commenterService)
        {
            _commenterService = commenterService;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            _commenterService.AddSqlComment(command, eventData);
            
            return result;
        }
        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<object> result)
        {
            _commenterService.AddSqlComment(command, eventData);
            return base.ScalarExecuting(command, eventData, result);
        }
        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData,
            InterceptionResult<int> result)
        {
            _commenterService.AddSqlComment(command, eventData);
            return base.NonQueryExecuting(command, eventData, result);
        }

        #if NET5_0_OR_GREATER
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            _commenterService.AddSqlComment(command, eventData);
            return new ValueTask<InterceptionResult<DbDataReader>>(Task.FromResult(result));
        }
        

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _commenterService.AddSqlComment(command, eventData);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _commenterService.AddSqlComment(command, eventData);

            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }
        #else
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            _commenterService.AddSqlComment(command, eventData);
            return Task.FromResult(result);
        }
    

        

        public override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _commenterService.AddSqlComment(command, eventData);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        

        public override Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _commenterService.AddSqlComment(command, eventData);

            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }
    #endif
        
    }
}