using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlCommenter
{
    public class AttributeCollector : IAttributeCollector
    {
        public Dictionary<string, string> GetAttributes(ActionContext context, CommandEventData eventData)
        {
            var attributes = new Dictionary<string, string>();
            attributes.Add("framework", $"ASP.NET");

            if (context != null)
                GetAttributesFromActionContext(context, attributes);
            else
                GetAttributesFromAddedTag(eventData, attributes);

            if (eventData?.Context?.Database?.ProviderName != null)
                attributes.Add("db_driver", eventData.Context.Database.ProviderName);

            return attributes;
        }

        private static void GetAttributesFromActionContext(ActionContext context, Dictionary<string, string> attributes)
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
        
        private static void GetAttributesFromAddedTag(CommandEventData eventData, Dictionary<string, string> attributes)
        {
            var reader = new StringReader(eventData.Command.CommandText);
            var line = reader.ReadLine()?.Trim();

            if (line != null && line.StartsWith("--") && line.Length > 3)
            {
                line = line.Substring(3); // Remove comment subfix '-- '
                var args = line.Split(',');

                var routeValue = args.FirstOrDefault(x => x.StartsWith("route="))?.Split('=')?[1];
                if (!string.IsNullOrWhiteSpace(routeValue))
                    attributes.Add("route", routeValue.Replace("'", ""));

                var controllerValue = args.FirstOrDefault(x => x.StartsWith("controller="))?.Split('=')?[1];
                if (!string.IsNullOrWhiteSpace(controllerValue))
                    attributes.Add("controller", controllerValue.Replace("'", ""));

                var actionValue = args.FirstOrDefault(x => x.StartsWith("action="))?.Split('=')?[1];
                if (!string.IsNullOrWhiteSpace(actionValue))
                    attributes.Add("action", actionValue.Replace("'", ""));
            }
        }
    }
}