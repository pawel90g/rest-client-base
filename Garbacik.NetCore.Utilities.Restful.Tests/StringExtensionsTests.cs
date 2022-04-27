using Garbacik.NetCore.Utilities.Restful.Extensions;
using Xunit;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void IsUpperTest()
    {
        Assert.True("A".IsUpper());
        Assert.False("a".IsUpper());
        Assert.True('A'.IsUpper());
        Assert.False('a'.IsUpper());
    }
    
    [Fact]
    public void IsLowerTest()
    {
        Assert.False("A".IsLower());
        Assert.True("a".IsLower());
        Assert.False('A'.IsLower());
        Assert.True('a'.IsLower());
    }

    [Fact]
    public void SplitByUpperCharTests()
    {
        var split = "aaaBBcccDaasdasdFFFRDFadcD".SplitByUpperChar();
        Assert.Equal(11, split.Length);
        Assert.Equal("aaa", split[0]);
        Assert.Equal("B", split[1]);
        Assert.Equal("Bccc", split[2]);
        Assert.Equal("Daasdasd", split[3]);
        Assert.Equal("F", split[4]);
        Assert.Equal("F", split[5]);
        Assert.Equal("F", split[6]);
        Assert.Equal("R", split[7]);
        Assert.Equal("D", split[8]);
        Assert.Equal("Fadc", split[9]);
        Assert.Equal("D", split[10]);
    }

    [Theory]
    [InlineData("ThisIsPascalCase", "this_is_pascal_case")]
    [InlineData("ThisIsPascalCase", "thisIsPascalCase")]
    [InlineData("ThisIsPascalCase", "ThisIsPascalCase")]
    public void ToPascalCaseTests(string expected, string input)
    {
        Assert.Equal(expected, input.ToPascalCase());
    }

    [Theory]
    [InlineData("thisIsCamelCase", "this_is_camel_case")]
    [InlineData("thisIsCamelCase", "thisIsCamelCase")]
    [InlineData("thisIsCamelCase", "ThisIsCamelCase")]
    public void ToCamelCaseTests(string expected, string input)
    {
        Assert.Equal(expected, input.ToCamelCase());
    }
}