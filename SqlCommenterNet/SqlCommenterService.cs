using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{
    public class SqlCommenterService:ISqlCommenterService
    {
        private readonly IAttributeCollector _attributeCollector;
        private readonly ICommentGenerator _commentGenerator;
        private readonly ActionContext _context;

        public SqlCommenterService(IActionContextAccessor contextAccessor,IAttributeCollector attributeCollector,ICommentGenerator commentGenerator)
        {
            _context = contextAccessor.ActionContext;
            _attributeCollector = attributeCollector;
            _commentGenerator = commentGenerator;
        }
        public void AddSqlComment(DbCommand command, CommandEventData eventData)
        {
            if (command == null) return;

            var attributes = _attributeCollector.GetAttributes(_context, eventData);

            var comment = _commentGenerator.GenerateSqlComment(attributes);
            _commentGenerator.ManipulateCommand(command, comment);
        }
       
    }
}