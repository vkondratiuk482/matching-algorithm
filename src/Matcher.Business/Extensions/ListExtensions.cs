namespace Matcher.Business.Extensions;

public static class ListExtensions
{
    public static T Pop<T>(this IList<T> list)
    {
        var index = list.Count - 1;
        
        var result = list[index];
        list.RemoveAt(index);
        
        return result;
    }
}
