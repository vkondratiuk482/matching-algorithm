namespace Matcher.Business.Interfaces;

public interface ICachingKey
{
    public string Prefix { get; }
    
    public string Value { get; }
}
