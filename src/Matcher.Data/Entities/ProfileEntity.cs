using Matcher.Business.Enums;

namespace Matcher.Data.Entities;

public class ProfileEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }
}
