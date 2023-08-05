using Matcher.Business.Enums;

namespace Matcher.Business.Core;

public class Profile
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }
}
