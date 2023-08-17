using Matcher.Business.Enums;

namespace Matcher.Business.Core;

public class MatchingMask
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Age { get; set; }

    public Genders? Gender { get; set; }
}
