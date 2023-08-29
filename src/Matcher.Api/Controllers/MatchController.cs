using Matcher.Business.Core;
using Matcher.Business.Enums;
using Matcher.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Matcher.Api.Controllers;

[ApiController]
[Route("api/match")]
public sealed class MatchController : Controller
{
    private readonly MatchService _matchService;

    public MatchController(MatchService matchService)
    {
        _matchService = matchService;
    }

    // Temp
    [HttpGet("{id}")]
    public async Task<Profile?> Get(int id,
        int? age,
        Genders gender)
    {
        var matchingCriteria = new MatchingCriteria
        {
            Age = age,
            Gender = gender,
        };

        return await _matchService.GetAsync(id, matchingCriteria);
    }
}
