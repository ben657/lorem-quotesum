using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    public MovieController()
    {
        
    }

    [HttpGet("{title}")]
    public ActionResult<Movie> GetMovie(string title)
    {
        return MovieService.GetMovie(title);
    }
}