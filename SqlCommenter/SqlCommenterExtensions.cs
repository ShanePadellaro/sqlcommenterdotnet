using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SqlCommenter
{

    public static class SqlCommenterExtensions
    {
        public static void AddSqlCommenter(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ICommentGenerator, CommentGenerator>();
            services.AddTransient<IAttributeCollector, AttributeCollector>();
            services.AddTransient<ISqlCommenterService, SqlCommenterService>();
            services.AddTransient<SqlCommenterInterceptor>();
        }
        
        public static DbContextOptionsBuilder UseSqlCommenter(this DbContextOptionsBuilder builder, IServiceProvider provider)
        {
            return builder.AddInterceptors(provider.GetService<SqlCommenterInterceptor>());
        }
    }
    
    
}