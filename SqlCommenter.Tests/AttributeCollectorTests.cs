using System.Collections.Generic;
using FluentAssertions;
using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace SqlCommenter.Tests;

public class AttributeCollectorTests
{
    private readonly AttributeCollector _underTest;

    public AttributeCollectorTests()
    {
        _underTest = new AttributeCollector();
    }

    [Fact]
    public void GetAttributesFromContext_Get_Controller()
    {
        var httpContext = new HttpContextMock()
            .SetupUrl("http://localhost:8000/path")
            .SetupRequestMethod("GET");
        var context = new ActionContext(httpContext,new RouteData(),new ActionDescriptor());
        context.RouteData = new RouteData(new RouteValueDictionary() {{"controller", "testController"}});
        var attributes = new Dictionary<string, string>();

        attributes = _underTest.GetAttributesFromContext(context, attributes);

        attributes["controller"].Should().Be("testController");
    }
    
    [Fact]
    public void GetAttributesFromContext_Get_Route()
    {
        var httpContext = new HttpContextMock()
            .SetupUrl("http://localhost:8000/my/route");
        
        var context = new ActionContext(httpContext,new RouteData(),new ActionDescriptor());
        context.RouteData = new RouteData(new RouteValueDictionary() {{"controller", "testController"}});
        var attributes = new Dictionary<string, string>();

        attributes = _underTest.GetAttributesFromContext(context, attributes);

        attributes["route"].Should().Be("/my/route");
    }
    
    [Fact]
    public void GetAttributesFromContext_Get_Action()
    {
        var httpContext = new HttpContextMock()
            .SetupUrl("http://localhost:8000/my/route");

        var context = new ActionContext(httpContext,new RouteData(),new ActionDescriptor());
        context.RouteData = new RouteData(new RouteValueDictionary() {{"action", "testAction"}});
        var attributes = new Dictionary<string, string>();

        attributes = _underTest.GetAttributesFromContext(context, attributes);
        attributes["action"].Should().Be("testAction");
    }
    
    [Fact]
    public void GetAttributesFromContext_Get_Traces()
    {
        var httpContext = new HttpContextMock()
            .SetupUrl("http://localhost:8000/my/route")
            .SetupRequestHeaders(new HeaderDictionary()
                {{"traceparent", "testTraceParent"}, {"tracestate", "testTraceState"}});

        var context = new ActionContext(httpContext,new RouteData(),new ActionDescriptor());
        var attributes = new Dictionary<string, string>();

        attributes = _underTest.GetAttributesFromContext(context, attributes);
        attributes["traceparent"].Should().Be("testTraceParent");
        attributes["tracestate"].Should().Be("testTraceState");

    }
}