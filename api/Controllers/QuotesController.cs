using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class QuotesController : ControllerBase
{
    public QuotesController()
    {
        
    }

    [HttpGet("titles")]
    public ActionResult<List<string>> GetTitles()
    {
        return MovieService.GetMovieTitles();
    }

    [HttpGet("{title}/quote")]
    public ActionResult<Quote> GetQuote(string title)
    {
        return MovieService.GetQuote(title);
    }

    [HttpGet("{title}/lines")]
    public ActionResult<List<Line>> GetLines(string title, int amount = 1)
    {
        return MovieService.GetLines(title, amount);
    }
}