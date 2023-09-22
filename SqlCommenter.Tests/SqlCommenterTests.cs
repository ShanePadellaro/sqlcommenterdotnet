using System.Collections.Generic;
using System.Text.Encodings.Web;
using FluentAssertions;
using Xunit;

namespace SqlCommenter.Tests;

public class SqlCommenterTests
{
    private readonly CommentGenerator _commenter;

    public SqlCommenterTests()
    {
        _commenter = new CommentGenerator();
    }
    // [Fact]
    // public void GenerateSqlComment_KeyAndValueIsURLEncoded()
    // {
    //     var attributes = new Dictionary<string, string>();
    //     attributes.Add("TestKey","TestValue");
    //     var comment = _commenter.GenerateSqlComment(attributes);
    //
    //     var key = UrlEncoder.Default.Encode("TestKey");
    //     var value = UrlEncoder.Default.Encode("TestValue");
    //     
    //     comment.Should().Be($"{key}={value}");
    // }

    [Fact]
    public void GenerateSqlComment_NullDictionary_Returns_Empty_String()
    {
        var dict = new Dictionary<string, string>();
        var comment = _commenter.GenerateSqlComment(dict);


        comment.Should().Be(string.Empty);
    }
    
    [Fact]
    public void GenerateSqlComment_OneAttribute()
    {
        var attributes = new Dictionary<string, string>();
        attributes.Add("TestKey","TestValue");
        var comment = _commenter.GenerateSqlComment(attributes);

        var key = UrlEncoder.Default.Encode("TestKey");
        var value = UrlEncoder.Default.Encode("TestValue");
        
        comment.Should().Be($"{key}='{value}'");
    }
    
    [Fact]
    public void GenerateSqlComment_With_Url_Encoding()
    {
        var attributes = new Dictionary<string, string>();
        attributes.Add("route","test/route");
        var comment = _commenter.GenerateSqlComment(attributes);
        
        comment.Should().Be("route='test%2Froute'");
    }
    
    [Fact]
    public void GenerateSqlComment_Multi_Tag()
    {
        var attributes = new Dictionary<string, string>();
        attributes.Add("route","test/route");
        attributes.Add("db_driver","npgsql");
        attributes.Add("action","test-action");

        var comment = _commenter.GenerateSqlComment(attributes);
        
        comment.Should().Be("route='test%2Froute',db_driver='npgsql',action='test-action'");
    }
}