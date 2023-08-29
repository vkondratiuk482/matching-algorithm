using Matcher.Business.Enums;

namespace Matcher.Business.Core;

public class ProfileCriteria
{
    public string? Name { get; set; }

    public int? Age { get; set; }

    public Genders? Gender { get; set; }

    public int? MinScore { get; set; }

    public int? MaxScore { get; set; }

    public string ToString()
    {
        var pieces = new List<string>();

        pieces.Add($"name:{Name}");
        pieces.Add($"age:{Age}");
        pieces.Add($"gender:{Gender}");
        pieces.Add($"minScore:{MinScore}");
        pieces.Add($"maxScore:{MaxScore}");

        return string.Join(":", pieces);
    }
}
