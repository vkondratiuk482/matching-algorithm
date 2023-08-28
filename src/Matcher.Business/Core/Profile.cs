using Matcher.Business.Enums;

namespace Matcher.Business.Core;

public class Profile
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Age { get; set; }

    public Genders Gender { get; set; }

    public int Score { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int UncommittedScore { get; set; }
}
