using Microsoft.AspNetCore.Mvc;

namespace Matcher.Api.Controllers;

[ApiController]
[Route("api/match")]
public sealed class MatchController : Controller
{
    [HttpGet]
    public async Task Get()
    {
        throw new NotImplementedException();
    }
}
