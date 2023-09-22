using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{
    public class AttributeCollector:IAttributeCollector
    {
        public Dictionary<string, string> GetAttributes(ActionContext context, CommandEventData eventData)
        {
            var attributes = new Dictionary<string, string>();
            attributes.Add("framework", $"ASP.NET");

            if (context != null)
                attributes = GetAttributesFromContext(context,attributes);


            if (eventData?.Context?.Database?.ProviderName != null)
                attributes.Add("db_driver", eventData.Context.Database.ProviderName);

            return attributes;

        }

        public Dictionary<string,string> GetAttributesFromContext(ActionContext context, Dictionary<string,string> attributes)
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
                attributes.Add("tracestate", tracestate.ToString());

            return attributes;
        }
    }
}