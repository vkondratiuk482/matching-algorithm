using Matcher.Business.Enums;

namespace Matcher.Api.Responses;

public class ProfileResponse
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }
}
