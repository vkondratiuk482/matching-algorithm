using Matcher.Business.Enums;

namespace Matcher.Data.Entities;

public class ProfileEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }

    public int Score { get; set; }

    public int UncommittedScore { get; set; }

    public int UserId { get; set; }
    public UserEntity? User { get; set; }
}
